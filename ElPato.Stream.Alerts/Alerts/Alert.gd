extends Control

@onready var what_label: Label = $Container/WhatLabel
@onready var who_label: Label = $Container/WhoLabel

func showAlert(what:String, who: String):
	what_label.text = what;
	who_label.text = who;
	$AnimationPlayer.play('show');
	await $AnimationPlayer.animation_finished;
	self.queue_free();
