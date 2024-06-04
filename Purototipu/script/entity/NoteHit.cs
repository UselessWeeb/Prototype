using Godot;
using System;

public partial class NoteHit : Area2D
{
	[Export]
	public int place { get; set; }
	
	[Export]
	public float speed { get; set; }
	
	public static int SPAWN_Y = -16;
	
	public static readonly Vector2 FIRST_LANE_SPAWN = new Vector2(305, SPAWN_Y);
	
	public static readonly Vector2 SECOND_LANE_SPAWN = new Vector2(495, SPAWN_Y);
	
	public static readonly Vector2 THIRD_LANE_SPAWN = new Vector2(678, SPAWN_Y);
	
	public static readonly Vector2 FORTH_LANE_SPAWN = new Vector2(890, SPAWN_Y);
	
	public NoteHit() : this(1, 0, 0){}

	public NoteHit(int place, float bpm, float offset)
	{
		this.place = place;
		this.speed = CalculateSpeed(bpm, offset);
		// Set the initial position based on the 'place' value
		switch (place)
		{
			case 1:
				GlobalPosition = FIRST_LANE_SPAWN;
				break;
			case 2:
				GlobalPosition = SECOND_LANE_SPAWN;
				break;
			case 3:
				GlobalPosition = THIRD_LANE_SPAWN;
				break;
			case 4:
				GlobalPosition = FORTH_LANE_SPAWN;
				break;
			default:
				GD.Print("Invalid place value");
				break;
		}
	}

	public float CalculateSpeed(float bpm, float offset)
	{
		return ((bpm / 60.0f) * offset) * 100;
	}
	
	public override void _Process(double delta)
	{
		GlobalPosition += new Vector2(0, speed * (float)delta);

		if (Position.Y > GetViewportRect().Size.Y)
		{
			QueueFree();
		}
	}
	
	public override String ToString()
	{
		return $"Place : {place}, Speed : {speed}";
	}
}
