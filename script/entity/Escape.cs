using Godot;
using System;
using huntedrl.script;

public class Escape : Node, IInteractable
{
	public override void _Ready()
	{
	}
	
	public void Interact(Entity initiator )=> this.LoadScene("success");
	
	public string Description => "Launch Escape Pod";
}
