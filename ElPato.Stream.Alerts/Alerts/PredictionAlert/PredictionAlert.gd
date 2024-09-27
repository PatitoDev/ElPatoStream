extends Node2D

const OUTCOME_SCENE = preload("res://Alerts/PredictionAlert/Outcome/Outcome.tscn")

@onready var alertContainer = $AlertContainer
@onready var title = $AlertContainer/Title
@onready var outcomeContainer = $AlertContainer/Outcomes
@onready var outcomeItemStartingPosition = $AlertContainer/OutcomeItemStartingPosition
@onready var patchImage = $AlertContainer/ResizedContainer/NinePatchRect
@onready var lineSeparator = $AlertContainer/LineSeparator

@onready var animationPlayer = $AnimationPlayer

var OUTCOME_ITEM_MARGIN = 50;
var CONTAINER_PADDING = 25;

func _ready():
	WS.OnPredictionBegin.connect(onBegin);
	WS.OnPredictionEnd.connect(onEnd);
	WS.OnPredictionProgress.connect(onProgress);
	WS.OnPredictionLock.connect(onLock);

func onBegin(data: Dictionary):
	for child in outcomeContainer.get_children():
		outcomeContainer.remove_child(child);
	
	title.text = data['content']['title'];
	await get_tree().create_timer(0.1).timeout;
	
	var titleHeight = title.size.y;
	print(titleHeight);
	lineSeparator.position.y = title.position.y + titleHeight + 15
	outcomeItemStartingPosition.position.y = lineSeparator.position.y + 10;
	
	var outcomes = data['content']['outcomes'];
	var outcomeIndex = 0;
	
	patchImage.size.y = (((outcomes as Array).size() * OUTCOME_ITEM_MARGIN) + CONTAINER_PADDING + titleHeight + CONTAINER_PADDING + 10 + 18) / 3; 
	
	for outcome in outcomes:
		var id = outcome['title'];
		var title = outcome['title'];
		var color = outcome['color'];
		
		var outcomeUI = OUTCOME_SCENE.instantiate();
		outcomeUI.name = id;
		outcomeContainer.add_child(outcomeUI);
		var colorAsEnum = OutcomeItem.OUTCOME_COLOR.BLUE;
		if (color == 'pink'):
			colorAsEnum = OutcomeItem.OUTCOME_COLOR.PINK;
		outcomeUI.setLabelAndColor(title, colorAsEnum);
		outcomeUI.position = Vector2(outcomeItemStartingPosition.position.x, outcomeItemStartingPosition.position.y + (OUTCOME_ITEM_MARGIN * outcomeIndex));
		outcomeIndex += 1;

	animationPlayer.play('Show');

func onProgress(data: Dictionary):
	# on prediction voting end close
	var outcomes = data['content']['outcomes'];
	(outcomes as Array).sort_custom(func (a, b): return b['channelPoints'] < a['channelPoints']);
	
	var totalPoints = getTotalPointsFromOutcome(outcomes);
	var childs = outcomeContainer.get_children();
	
	for child in childs:
		var found = getByIdFromOutcomeArray(child.name, outcomes);
		# TODO - clean redundant code
		var index = outcomes.find(found);
		print(index)
		var newYPosition = outcomeItemStartingPosition.position.y + (OUTCOME_ITEM_MARGIN * index);
		create_tween().tween_property(child, 'position:y', newYPosition, 0.2);
		
		if (found == null):
			continue;
		
		var points = found['channelPoints'];
		child.updatePoints(points, points / totalPoints);

func onLock(data: Dictionary):
	await get_tree().create_timer(15).timeout;
	animationPlayer.play_backwards('Show');

func onEnd(data: Dictionary):
	pass;

func getTotalPointsFromOutcome(outcomes: Array):
	var total = 0;
	for outcome in outcomes:
		total += outcome['channelPoints'];
	return total;

func getByIdFromOutcomeArray(id: String, outcomes: Array):
	for outcome in outcomes:
		if (outcome['title'] == id):
			return outcome;
