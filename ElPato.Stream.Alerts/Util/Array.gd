extends Node

class_name ArrayUtil

static func average(arr: Array):
	var avg = Vector2(0,0);
	for i in range(arr.size()):
		avg += arr[i]
	avg /= arr.size()
	return avg
