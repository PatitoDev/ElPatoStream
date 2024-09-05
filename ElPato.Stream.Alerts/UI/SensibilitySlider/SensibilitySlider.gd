extends PanelContainer


@export var minValue: int = 0;
@export var maxValue: int = 100;
@export var stepBy: int = 1;

@onready var progressBar = $ProgressBar;
@onready var slider = $PanelContainer/VSlider;
@export var sensibilityValue = 50;
@export var volumeValue = 50;

signal onSensibilityChanged(value: float);

func _ready() -> void:
	progressBar.min_value = minValue;
	progressBar.max_value = maxValue;
	slider.value = sensibilityValue;
	progressBar.value = volumeValue;

func setVolume(volumne: int):
	progressBar.value = volumne;

func setSensibility(value: int):
	sensibilityValue = value;
	slider.value = value;

func _on_v_slider_value_changed(value: float) -> void:
	onSensibilityChanged.emit(value);
