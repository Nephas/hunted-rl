using Godot;
using System;
using System.Linq;
using huntedrl.script;

public class PC : Node
{
	private Actor _actor;

	public override void _Ready()
	{
		_actor = this.GetEntity().GetComponent<Actor>();

		_actor.Connect("OnDeath", this, "OnPCDeath");
	}

	private void OnPCDeath()
	{
		GetTree().ChangeScene("res://scenes/death.tscn");
	}
}
