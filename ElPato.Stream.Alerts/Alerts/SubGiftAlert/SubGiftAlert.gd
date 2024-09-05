extends Node2D

var queue = [];

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	WS.OnSubGift.connect(onSubGift);

func onSubGift(data: Dictionary):
	if $AnimationPlayer.is_playing():
		queue.push_back(data);
		return;
	play(data);

func play(data: Dictionary):
	var user = data['content'].get('userName');
	if user == null:
		user = 'Usuario Anonimo';
	var total = data['content']['total'];
	$Container/VSplitContainer/User.text = user;
	$Container/VSplitContainer/Amount.text = str(total);
	$AnimationPlayer.play('show');

func _on_animation_player_animation_finished(anim_name: StringName) -> void:
	if queue.size():
		play(queue.pop_front());
