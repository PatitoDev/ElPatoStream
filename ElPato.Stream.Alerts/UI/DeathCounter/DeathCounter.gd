extends Node2D

var value = 0;
var CONTAINER_STRING_OFFSETT = 10;
@onready var label: Label = $LabelContainer/Label;
@onready var nine_patch_rect: NinePatchRect = $ResizedContainer/NinePatchRect
var initialPosition = 0;

func _ready() -> void:
	WS.OnDeathCounterIncrease.connect(onCounterIncrease);
	WS.OnDeathCounterReset.connect(onCounterReset);
	label.text = str(value);
	initialPosition = nine_patch_rect.position.x;
	self.position.x = -200;

func onCounterIncrease(data: Dictionary):
	var amountToIncrease = data['content']['amount'];
	value = max(value + amountToIncrease, 0);
	label.text = str(value);
	repositionContainer();

func onCounterReset(data: Dictionary):
	value = 0;
	label.text = str(value);
	repositionContainer();

func repositionContainer():
	var size = (label.text as String).length() - 1;
	var newXPosition = (size * CONTAINER_STRING_OFFSETT) + initialPosition;
	var tween = get_tree().create_tween();
	tween.tween_property(nine_patch_rect, 'position:x', newXPosition, 0.2);

	var tween2 = get_tree().create_tween();
	print(self.position.x);
	if (value && self.position.x != 0):
		print(1);
		tween2.tween_property(self, 'position:x', 0, 0.2);

	if (!value && self.position.x == 0):
		print(2);
		tween2.tween_property(self, 'position:x', -200, 0.2);
