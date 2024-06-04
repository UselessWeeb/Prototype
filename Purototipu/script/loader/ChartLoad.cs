using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

public partial class ChartLoad
{
	[Export]
	String path { get; set; }
	
	public ChartLoad() : this("") {}
	
	public ChartLoad(String path){
		this.path = ProjectSettings.GlobalizePath(path);
	}
	
	public List<Beat> Load()
	{
		var beats = new List<Beat>();
		var notePattern = @"\((\d+), (\d+), (\w+), (\d+)\)";
		var bpmPattern = @"BPM:(\d+)\{";
		var fileContent = File.ReadAllText(path);
		
		// Split the content by BPM sections
		var bpmSections = Regex.Split(fileContent, bpmPattern, RegexOptions.Multiline);
		
		for (int i = 1; i < bpmSections.Length; i += 2)
		{
			var bpm = float.Parse(bpmSections[i]);
			var notesText = bpmSections[i + 1];
			var matches = Regex.Matches(notesText, notePattern);
			var notes = new List<Note>();
			
			foreach (Match match in matches)
			{
				var place = int.Parse(match.Groups[1].Value);
				var noteValue = float.Parse(match.Groups[2].Value);
				var type = match.Groups[3].Value;
				var duration = float.Parse(match.Groups[4].Value);
				
				notes.Add(new Note(place, noteValue, type, duration));
			}
			
			beats.Add(new Beat(bpm, notes));
		}
		
		return beats;
	}
}
