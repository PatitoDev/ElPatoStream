extends Node2D

@onready var streamer_name_label: Label = $Container/Control/VBoxContainer/StreamerNameLabel
@onready var category_label: Label = $Container/Control/VBoxContainer/CategoryLabel
@onready var title_label: Label = $Container/Control/VBoxContainer/TitleLabel
@onready var profile_image: TextureRect = $Container/ProfileImage
@onready var animation_player: AnimationPlayer = $AnimationPlayer

func _ready() -> void:
	WS.OnShoutoutCreate.connect(_onShoutout);

func _onShoutout(data: Dictionary):
	streamer_name_label.text = data['content']['userName'];
	category_label.text = data['content']['gameName'];
	title_label.text = data['content']['title'];
	var imageData:String = data['content']['profileImgAsBase64'];
	loadImageFromBase64(imageData);
	animation_player.play('Show');

func loadImageFromBase64(imageData: String):
	if !imageData.length():
		return;
	var image = Image.new()
	var imageParts = imageData.split(",", true, 1);
	var buff = Marshalls.base64_to_raw(imageParts[1]);
	var error;
	if (imageParts[0] as String).contains('jpeg'):
		error = image.load_jpg_from_buffer(buff);
	else:
		error = image.load_png_from_buffer(buff);

	if error != OK:
		push_error("Couldn't load the image.")
	var texture = ImageTexture.new()
	texture.set_image(image)
	profile_image.texture = texture;
