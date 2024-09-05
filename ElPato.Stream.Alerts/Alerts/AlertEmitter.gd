extends Node2D

var bitAlertScene = preload('res://Alerts/BitsAlert/BitsAlert.tscn');
@onready var spawn_point: Marker2D = $SpawnPoint

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	WS.OnBitEvent.connect(onBitEvent);

func onBitEvent(data: Dictionary):
	var amount = data['content']['bits'];
	var userName = data['content']['userName'];
	var scene = bitAlertScene.instantiate();
	addChildToRandomPosition(scene);

	scene.showBit(userName, amount);

func addChildToRandomPosition(nodeToAdd: Node2D):
	var children = get_children();
	for child in children:
		if !child.get_child_count():
			child.add_child(nodeToAdd);
			return;
	$SpawnPoint.add_child(nodeToAdd);
