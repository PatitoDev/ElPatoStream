extends Node2D


# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	WS.OnHeartRateDataReceive.connect(onHeartRate);

func onHeartRate(data: Dictionary):
	var bpm = str(data['content']);
	print(bpm);
	$Label.text = bpm;
