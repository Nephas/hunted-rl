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
		//UpdateFoV();
	}

	private void OnPCDeath()
	{
		GetTree().ChangeScene("res://scenes/death.tscn");
	}

	public void UpdateFoV()
	{
		var space = GameWorld.Get().GetWorld2d().DirectSpaceState;
		var otherEntities = GameWorld.Get().GetAllEntities().Where(e => !e.IsInGroup("pc"));
		var pc = GameWorld.Get().GetPC();

		foreach (var target in otherEntities)
		{
			var result = space.IntersectRay(pc.Position, target.Position);
			target.Visible = result.Count == 0;
		}
	}
}
