extends Node2D

enum AlertType {
	Subscriber,
	Follower
}

var rowScene = preload('res://Alerts/FollowerAlert/Row.tscn');
@onready var spawnContainer: Node2D = $AlertContainer/Panel/SpawnContainer
@onready var marker2D: Marker2D = $AlertContainer/Marker2D

var followerQueue = [];
var isRowAnimating = false;
var isShowing = false;
var closeTimer:SceneTreeTimer;
var followersAddedCounter = 0;
@export var type: AlertType = AlertType.Subscriber;

func _ready() -> void:
	match type:
		AlertType.Subscriber:
			$AlertContainer/ScaledContainer/FollowerWindow.visible = false;
			#WS.OnNewSubscriber.connect(pushAlert);
			WS.OnSubMessage.connect(pushAlert);
		AlertType.Follower:
			WS.OnNewFollower.connect(pushAlert);

func pushAlert(event: Dictionary):
	if $AnimationPlayer.is_playing():
		await $AnimationPlayer.animation_finished;

	if !isShowing:
		$AnimationPlayer.play('Show');
		await $AnimationPlayer.animation_finished;
		isShowing = true;

	_addRow(event);

func getDataFromEvent(event: Dictionary):
	match event['type']:
		'resub':
			var message = ' con un total de ' + str(event['content']['totalMonths']) + ' meses ';
			if (event['content']['message']):
				message = message + ' y dice, ' + event['content']['message'];

			return {
				'userName': event['content']['userName'],
				'message':  message
			};
		'new-follower':
			return {
				'userName': event['content']['name'],
			};
		'new-subscriber':
			return {
				'userName': event['content']['userName'],
			};

func _addRow(event: Dictionary):
	if closeTimer:
		closeTimer.disconnect('timeout', onCloseTimerEnd);
		closeTimer = null;

	if isRowAnimating:
		followerQueue.push_back(event);
		return;

	isRowAnimating = true;
	var row = rowScene.instantiate();
	spawnContainer.add_child(row);
	var data = getDataFromEvent(event);
	row.find_child('Label').text = data.userName.substr(0, 25);
	row.global_position = marker2D.global_position + Vector2(300, 0);
	create_tween().tween_property(row, 'global_position', marker2D.global_position, 0.2);
	$AudioStreamPlayer.play();
	for subRow in spawnContainer.get_children():
		if subRow == row:
			continue;
		create_tween().tween_property(subRow, 'global_position', subRow.global_position + Vector2(0, 35), 0.2);

	followersAddedCounter += 1;
	if followersAddedCounter < 3:
		var content = {
			'body': 'Nuevo Follower, ' + data.userName.substr(0, 12),
			'priority': 'high'
		};
		if type == AlertType.Subscriber:
			content.body = 'Nuevo subscriber, ' + data.userName.substr(0, 12);
			if data.get('message'):
				content.body += ', ' + data.message;
		WS.sendEvent('tts-message', content);
	await get_tree().create_timer(0.3).timeout;
	_onEnd();

func _onEnd():
	isRowAnimating = false;
	if followerQueue.size():
		_addRow(followerQueue.pop_front());
		return;

	closeTimer = get_tree().create_timer(8);
	closeTimer.timeout.connect(onCloseTimerEnd);

func onCloseTimerEnd():
	if followerQueue.size():
		_addRow(followerQueue.pop_front());
		return;

	if isShowing:
		$AnimationPlayer.play_backwards('Show');
		isShowing = false;
		if followersAddedCounter > 3:
			var content = {
				'body': 'Gracias por los ' + str(followersAddedCounter) + ' followers.',
				'priority': 'high'
			};
			if type == AlertType.Subscriber:
				content.body = 'Gracias por los ' + str(followersAddedCounter) + ' subscribers.'
			WS.sendEvent('tts-message', content);
		followersAddedCounter = 0;
