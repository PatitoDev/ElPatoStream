extends CanvasLayer

const SETTINGS_FILE_PATH = "user://settings.save";
var settings:SettingState = SettingState.new();
signal onSettingsChanged(settings: SettingState);
var hasLoaded = false;

func _ready():
	self.settings = _loadSettings();
	onSettingsChanged.emit(self.settings);
	self.visible = self.settings.isSettingsVisible;
	hasLoaded = true;

func updateMouseMode():
	if self.visible:
		DisplayServer.mouse_set_mode(DisplayServer.MOUSE_MODE_VISIBLE);
	else:
		DisplayServer.mouse_set_mode(DisplayServer.MOUSE_MODE_HIDDEN);

func _process(delta: float):
	if (Input.is_action_just_pressed("ui_menu")):
		self.visible = !self.visible;
		self.settings.isSettingsVisible = self.visible;
		updateMouseMode();
		_saveSettings();

func _loadSettings():
	if FileAccess.file_exists(SETTINGS_FILE_PATH):
		print("Settings found. Loading saved state...");
		var file = FileAccess.open(SETTINGS_FILE_PATH, FileAccess.READ)
		var loadedFile: Variant = file.get_as_text();
		file.close();
		var data = JSON.parse_string(loadedFile);
		print(data);
		hasLoaded = true;
		return SettingState.fromJson(data);
	else:
		print("Settings not found")
		return SettingState.new();

func _saveSettings():
	var file = FileAccess.open(SETTINGS_FILE_PATH, FileAccess.WRITE);
	var json = self.settings.toJson();
	file.store_string(json);
	file.close();

func _on_close_btn_pressed() -> void:
	self.settings.isSettingsVisible = false;
	self.visible = false;
	updateMouseMode();
	_saveSettings();
