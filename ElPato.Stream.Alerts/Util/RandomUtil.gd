extends Node

class_name RandomUtil;

static func generateRandomVector():
	var x = randi_range(50, 500);
	if (randf() > 0.5):
		x = -x;
	var y = randi_range(200, 800);
	if (randf() > 0.5):
		y = -y;
	return Vector2(x,y);
