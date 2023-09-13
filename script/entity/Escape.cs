using Godot;
using System;

public class Escape : Node, IInteractable
{
	public override void _Ready()
	{
	}
	
	public void Interact(Entity initiator)
	{
		GetTree().ChangeScene("res://scenes/success.tscn");
	}
	
	[Export]
	public string Description => "Launch Escape Pod";
}
