using Godot;
using System;

public partial class Play : Node
{
	[Export]
	public NodePath conductorPath;
	
	[Export]
	public int audio_offset = 1;
	
	private Conductor _conductor;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_conductor = GetNode<Conductor>("Conductor");
		_conductor.PlayWithBeatOffset(audio_offset);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
