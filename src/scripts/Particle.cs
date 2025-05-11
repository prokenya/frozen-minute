using Godot;
using System;

public partial class Particle : GpuParticles2D
{
	private AudioStreamPlayer player;
	public override void _Ready()
	{
		Emitting = true;
		player = GetNode<AudioStreamPlayer>("%AudioStreamPlayer");
		GD.Randomize();
		float randomValue = 0.8f + GD.Randf() * (1.2f - 0.8f);
		player.PitchScale = randomValue;
	}
	private void _on_finished(){
		QueueFree();
	}
}
