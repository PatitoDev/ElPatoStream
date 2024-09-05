extends Node2D;

class_name SettingState

@export var selectedInputDevice: String;
@export var sensibility: int = 25;
@export var isSettingsVisible: bool = false;

func toJson():
	return JSON.stringify({
		sensibility = self.sensibility,
		selectedInput = self.selectedInputDevice,
		isSettingsVisible = self.isSettingsVisible
	});

static func fromJson(data: Dictionary):
	var loadedSettings = SettingState.new();
	loadedSettings.sensibility = data['sensibility'];
	loadedSettings.selectedInputDevice = data['selectedInput'];
	loadedSettings.isSettingsVisible = data['isSettingsVisible'];
	return loadedSettings;
