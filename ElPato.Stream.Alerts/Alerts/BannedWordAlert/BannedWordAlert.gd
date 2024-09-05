extends Node2D

@onready var bannedWordLabel: Label = $Alert/UI/Label2

func _ready():
	WS.OnBanWordReward.connect(_onBanWordEvent);
	WS.OnBanWordReset.connect(_onBanWordReset);

func _onBanWordEvent(data: Dictionary):
	print(data);
	var bannedWord = data['content']['userInput'];
	WS.ShowMarquee.emit("ATENCION: Escribir lo siguiente te ganas un timeout de 10 minutos: '" + bannedWord + "' | ");
	bannedWordLabel.text = bannedWord;
	$AnimationPlayer.play('show');

func _onBanWordReset(data: Dictionary):
	WS.HideMarquee.emit();
