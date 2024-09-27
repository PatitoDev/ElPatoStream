extends Node

@export var url = "ws://localhost:5131";
var client = WebSocketPeer.new();

signal ShowMarquee(text: String);
signal HideMarquee();

signal OnConnection;
# Twitch events
signal OnBitEvent;
signal OnNewFollower;
signal OnRaidEvent;
signal OnNewSubscriber;
signal OnSubMessage;
signal OnVipRedeem;
signal OnSubGift;

signal OnPredictionBegin;
signal OnPredictionProgress;
signal OnPredictionEnd;
signal OnPredictionLock;

# Commands
signal OnDeathCounterReset;
signal OnDeathCounterIncrease;
signal OnShoutoutCreate;

var hasConnected = false;

var messageDictionary = {
	'bit-event': OnBitEvent,
	"new-follower": OnNewFollower,
	"raid-event": OnRaidEvent,
	"new-subscriber": OnNewSubscriber,
	'resub': OnSubMessage,
	'channel-shoutout-create': OnShoutoutCreate,
	'reset-death-counter': OnDeathCounterReset,
	'increase-death-counter': OnDeathCounterIncrease,
	'vip-redeem-success': OnVipRedeem,
	'sub-gift': OnSubGift,
	'prediction-begin': OnPredictionBegin,
	'prediction-progress': OnPredictionProgress,
	'prediction-end': OnPredictionEnd,
	'prediction-lock': OnPredictionLock
}

func _process(delta):
	client.poll()
	var state = client.get_ready_state()
	if state == WebSocketPeer.STATE_OPEN:
		if (!hasConnected):
			onConnected();
		while client.get_available_packet_count():
			onReceivedData(client.get_packet());
	elif state == WebSocketPeer.STATE_CLOSING:
		print('Closing ws');
	elif state == WebSocketPeer.STATE_CLOSED:
		var code = client.get_close_code()
		var reason = client.get_close_reason()
		print("WebSocket closed with code: %d, reason %s. Clean: %s" % [code, reason, code != -1])
		set_process(false) # Stop processing.

func _ready():
	client.inbound_buffer_size = 65535 * 1000;
	var err = client.connect_to_url(url);
	if err != OK:
		print("Unable to connect");
		set_process(false);

func onConnected():
	hasConnected = true;
	print("Connected to ws at ", url);
	OnConnection.emit();

func onReceivedData(payload: PackedByteArray):
	var message = payload.get_string_from_utf8()
	print(message);
	var data = JSON.parse_string(message);
	if !data:
		return;
	var type:String = data["type"];
	print("Got message from server: ", type);
	var event = messageDictionary.get(type);
	if (event == null):
		return;
	event.emit(data);

func sendEvent(type: String, content = null):
	var payload = {
		"type": type,
	};

	if (content != null):
		payload['content'] = content;

	var msg = JSON.stringify(payload);
	client.send_text(msg);
