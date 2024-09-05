extends Node2D

var queue = [];

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	WS.OnVipRedeem.connect(onVipIncreased);

func onVipIncreased(data: Dictionary):
	if $AnimationPlayer.is_playing():
		queue.push_back(data);
		return;
	play(data);

func play(data: Dictionary):
	var user = data['content']['userName'];
	var newAmount = data['content']['increasedTo'];
	$Container/Extra/UserNameLabel.text = user;
	$Container/Extra/CostLabel.text = str(newAmount);
	$AnimationPlayer.play('show');

func _on_animation_player_animation_finished(anim_name: StringName) -> void:
	if queue.size():
		play(queue.pop_front());
