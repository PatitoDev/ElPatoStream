extends Node2D


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	#WS.OnSingReward.connect(onSingRewardRequest);
	pass;

func onSingRewardRequest(data: Dictionary):
	$AnimationPlayer.play('Show');
