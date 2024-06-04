using Godot;
using System;

public partial class ButtonHandler : Area2D
{
	Texture2D _clicked;
	Texture2D _unclicked;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_clicked = GD.Load<Texture2D>("res://sprite/Red-idk.png");
		_unclicked = GD.Load<Texture2D>("res://sprite/Blue-idk.png");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	private void _on_input_event(Node viewport, InputEvent @event, long shape_idx)
	{
		if (@event is InputEventMouseButton mouseEvent)
			{
				var button = GetNode<Sprite2D>("Blue-idk");
				
				if (mouseEvent.Pressed)
				{
					// The object was clicked
					button.Texture = _clicked;
					GD.Print("The object was clicked!");
				}
				else
				{
					// The mouse button was released
					button.Texture = _unclicked;
					GD.Print("The mouse button was released.");
				}
			}
	}
}
