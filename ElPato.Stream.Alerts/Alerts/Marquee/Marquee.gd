extends Node2D

@onready var marqueeLabel: Label = $Container/ColorRect/Label;
@onready var marqueeLabel2: Label = $Container/ColorRect/Label2;

const MARQUEE_SPEED = 0.5;
const LABEL_OFFSETT = 1;

@export var text = "Algo";
var marqueeIsMarquing = false;

func _ready() -> void:
	WS.ShowMarquee.connect(showMarquee);
	WS.HideMarquee.connect(hideMarquee);

func _on_timer_timeout() -> void:
	if !marqueeIsMarquing:
		return;

	var clipContainer = marqueeLabel.get_parent();

	marqueeLabel.position.x -= MARQUEE_SPEED;
	if marqueeLabel.position.x < -marqueeLabel.size.x:
		marqueeLabel.position.x = marqueeLabel2.position.x + marqueeLabel2.size.x + LABEL_OFFSETT;

	marqueeLabel2.position.x -= MARQUEE_SPEED;
	if marqueeLabel2.position.x < -marqueeLabel2.size.x:
		marqueeLabel2.position.x = marqueeLabel.position.x + marqueeLabel.size.x + LABEL_OFFSETT;

func showMarquee(textToDisplay: String):
	text = textToDisplay;
	marqueeLabel.text = text;
	marqueeLabel.position.x = marqueeLabel.get_parent().size.x;

	marqueeLabel2.text = text;
	marqueeLabel2.position.x = marqueeLabel.position.x + marqueeLabel.size.x + LABEL_OFFSETT;
	marqueeIsMarquing = true;
	$AnimationPlayer.play('show');

func hideMarquee():
	marqueeIsMarquing = false;
	$AnimationPlayer.play_backwards('show');

