using Godot;
using System;

public partial class Note
{
	[Export]
	public int place { get; set; }
	
	[Export]
	public double note { get; set; }
	
	[Export]
	public String type { get; set; }

	[Export]
	public float duration { get; set; }
	
	public Note() : this(0, 0, "", 0.0f) {}

	public Note(int place, double note, String type, float duration)
	{
		this.place = place;
		this.note = note;
		this.type = type;
		this.duration = duration;
	}
	
	public override string ToString()
	{
		return $"Note: {note}, Place : {place}, Type: {type}, Duration: {duration}";
	}
}
