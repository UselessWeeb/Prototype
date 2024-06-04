using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Conductor : AudioStreamPlayer
{
public int Bpm;
	
public int Measures = 4;

//idk we might needs to ask player to re-enter the offset when first enter the game, I learn form MuseDash lmfao
public float _offset = 4f;

[Export]
public String Path;

private double _songPosition = 0.0;
private int _songPositionInBeats = 1;
private double _secPerBeat;
private int _lastReportedBeat = 0;
private int _beatsBeforeStart = 0;
private int _measure = 1;

private int _closest = 0;
private double _timeOffBeat = 0.0;

private List<Note> _notesSpawned = new List<Note>();



[Signal]
public delegate void BeatEventHandler(int position);
[Signal]
public delegate void MeasureEventHandler(int position);

//pre-making note for better idk efficiency
private PackedScene noteHitScene = (PackedScene)ResourceLoader.Load("res://scene/Note.tscn");

private ChartLoad ChartLoad;
private List<Beat> Chart;
private int CurrentBeatIndex = 0;

	public override void _Ready()
	{
		ChartLoad = new ChartLoad(Path);
		Chart = ChartLoad.Load();
		SetBpm(Chart[CurrentBeatIndex].BPM); // Set initial BPM
	}

	public override void _Process(double delta)
	{
		if (Playing)
		{
			_songPosition = GetPlaybackPosition() + AudioServer.GetTimeSinceLastMix();
			_songPosition -= AudioServer.GetOutputLatency();
			_songPositionInBeats = (int)Math.Floor(_songPosition / _secPerBeat) + _beatsBeforeStart;
			_ReportBeat();
		}
	}

	private void _ReportBeat()
	{
		if (_lastReportedBeat < _songPositionInBeats)
		{
			if (_measure > Measures)
			{
				_measure = 1;
			}

			if (CurrentBeatIndex < Chart.Count)
			{
				Beat currentBeat = Chart[CurrentBeatIndex];
				foreach (Note note in currentBeat.Chart)
				{
					if (note.note == _songPositionInBeats && !_notesSpawned.Contains(note))
					{
						double timeUntilNextNote = (note.note - _songPositionInBeats) * _secPerBeat;
						double noteSpawnTime = _songPosition - timeUntilNextNote + (_offset / 10.0f);
						GD.Print(note);
						_notesSpawned.Add(note);
					}
				}
				ProcessNotes();
				if (_songPositionInBeats >= Chart[CurrentBeatIndex].Chart.Last().note)
				{
					CurrentBeatIndex++;
					if (CurrentBeatIndex < Chart.Count)
					{
						SetBpm(Chart[CurrentBeatIndex].BPM);
					}
				}
			}

			_lastReportedBeat = _songPositionInBeats;
			_measure += 1;
		}
	}

	private void ProcessNotes()
	{
		for (int i = _notesSpawned.Count - 1; i >= 0; i--)
		{
			Note note = _notesSpawned[i];
			if (_songPositionInBeats >= note.note - (_offset / 100.0f))
			{
				CreateNote(note.place, Bpm, _offset);
				_notesSpawned.RemoveAt(i);
			}
		}
	}

	private void CreateNote(int place, float bpm, float offset)
	{
		NoteHit newNote = (NoteHit)noteHitScene.Instantiate();
		newNote.place = place;
		newNote.speed = newNote.CalculateSpeed(bpm, offset);
		switch (place)
		{
			case 1:
				newNote.GlobalPosition = NoteHit.FIRST_LANE_SPAWN;
				break;
			case 2:
				newNote.GlobalPosition = NoteHit.SECOND_LANE_SPAWN;
				break;
			case 3:
				newNote.GlobalPosition = NoteHit.THIRD_LANE_SPAWN;
				break;
			case 4:
				newNote.GlobalPosition = NoteHit.FORTH_LANE_SPAWN;
				break;
			default:
				GD.Print("Invalid place value");
				break;
		}
		AddChild(newNote);
	}

	private void SetBpm(float bpm)
	{
		Bpm = (int)bpm;
		_secPerBeat = 60.0 / Bpm;
	}

	public void PlayWithBeatOffset(int num)
	{
		_beatsBeforeStart = num;
		var startTimer = GetNode<Timer>("StartTimer");
		startTimer.WaitTime = (float)(_secPerBeat * _beatsBeforeStart);
		startTimer.Start();
	}

	public Vector2 ClosestBeat(int nth)
	{
		_closest = (int)Math.Round((_songPosition / _secPerBeat) / nth) * nth;
		_timeOffBeat = Math.Abs(_closest * _secPerBeat - _songPosition);
		return new Vector2(_closest, (float)_timeOffBeat);
	}

	public void PlayFromBeat(int beat, int offset)
	{
		Play();
		Seek(beat * (float)_secPerBeat);
		_beatsBeforeStart = offset;
		_measure = beat % Measures;
	}

	private void _on_start_timer_timeout()
	{
		_songPositionInBeats += 1;
		var startTimer = GetNode<Timer>("StartTimer");
		if (_songPositionInBeats < _beatsBeforeStart - 1)
		{
			startTimer.Start();
		}
		else if (_songPositionInBeats == _beatsBeforeStart - 1)
		{
			startTimer.WaitTime = (float)(startTimer.WaitTime - (AudioServer.GetTimeToNextMix() +
																 AudioServer.GetOutputLatency()));
			startTimer.Start();
		}
		else
		{
			Play();
			startTimer.Stop();
			_ReportBeat();
		}
	}
}
