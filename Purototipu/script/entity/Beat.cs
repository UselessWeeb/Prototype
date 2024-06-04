using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public partial class Beat
{
	[Export]
	public float BPM { get; set; }
	
	public List<Note> Chart { get; set; }
	
	public Beat() : this(0f, new List<Note>()) {}
	
	public Beat(float BPM, List<Note> Chart){
		this.BPM = BPM;
		this.Chart = Chart;
	}
	
	public override string ToString()
	{
		var notesString = String.Join(", ", Chart.Select(note => note.ToString()));
		return $"BPM: {BPM}, Notes: [{notesString}]";
	}
}
