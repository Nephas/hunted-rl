using Godot;
using System;

public class Input : Node
{
	public override void _Ready()
	{
		
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("up"))
		{
			GD.Print("my_action occurred!");
		}
	}
}
