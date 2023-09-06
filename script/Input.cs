using Godot;
using System;

public class Input : Node
{
	private Entity _pc;

	public override void _Ready()
	{
		_pc = GetTree().GetNodesInGroup("pc")[0] as Entity;
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("up")) _pc.WorldPos += Vector2.Up;
		if (@event.IsActionPressed("down")) _pc.WorldPos += Vector2.Down;
		if (@event.IsActionPressed("left")) _pc.WorldPos += Vector2.Left;
		if (@event.IsActionPressed("right")) _pc.WorldPos += Vector2.Right;
	}
}
