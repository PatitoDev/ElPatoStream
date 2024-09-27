extends Node2D
class_name OutcomeItem;

@onready var outcomeTitle = $OutcomeTitle
@onready var outcomePoints = $OutcomePoints
@onready var outcomeBar = $OutcomeBar

# 6 real pixels = 2 pixel art = 1 unit
var UNIT_SIZE = 12;
var MAX_WIDTH = 276 / UNIT_SIZE;

enum OUTCOME_COLOR {
	BLUE,
	PINK
}

func _ready():
	updatePoints(0,0);

func setLabelAndColor(title:String, color: OUTCOME_COLOR):
	outcomeTitle.text = title;
	if (color == OUTCOME_COLOR.BLUE):
		outcomeBar.color = Color.from_string("#5274e1", Color.WHITE);

func updatePoints(points: int, percentageTotal: float):
	outcomePoints.text = str(points);
	var size = max((MAX_WIDTH * percentageTotal), 2) * UNIT_SIZE;
	if (points == 0):
		size = 1 * UNIT_SIZE;
	
	create_tween().tween_property(outcomeBar, 'size:x', size, 0.5);
