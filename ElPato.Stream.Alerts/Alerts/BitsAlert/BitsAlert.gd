extends Node2D

@onready var particle_container: Node2D = $Alert/ParticleContainer
var bitStringTemplate = '[rainbow freq=1.0 sat=0.8 val=0.8][center][wave amp=50.0 freq=5.0 connected=1]';
@onready var multi_coin_sfx: AudioStreamPlayer = $MultiCoinSFX
@onready var gpu_particles_2d: GPUParticles2D = $Alert/ParticleContainer/GPUParticles2D

func showBit(userName: String, amount: int):
	var multicoin = amount > 5;

	$Alert/RichTextLabel.text = bitStringTemplate + str(amount);
	$Alert/Label.text = userName;

	$AnimationPlayer.play('Show');
	if !multicoin:
		for i in amount:
			var newParticle = gpu_particles_2d.duplicate();
			particle_container.add_child(newParticle);
			newParticle.amount = 1;
			newParticle.emitting = true;

			var newSFX = $SingleCoinSFX.duplicate();
			add_child(newSFX);
			newSFX.play();
			await get_tree().create_timer(0.2).timeout;
		onEndOfEmitting();
		return;

	for i in range(ceil(amount / 5) + 1):
		var newParticle = gpu_particles_2d.duplicate();
		particle_container.add_child(newParticle);
		newParticle.amount = 5;
		newParticle.emitting = true;

		var newSFX = multi_coin_sfx.duplicate();
		add_child(newSFX);
		newSFX.play();
		await get_tree().create_timer(0.2).timeout;
	onEndOfEmitting();

func _on_animation_player_animation_finished(anim_name: StringName) -> void:
	if anim_name == 'Hide':
		queue_free();

func onEndOfEmitting():
	await get_tree().create_timer(3).timeout;
	$AnimationPlayer.play('Hide');
