using Godot;
using System;
using huntedrl.script;

public class Push : Node, IInteractable
{
	public void Interact(Entity initiator)
	{
		var entity = this.GetEntity();
		var dir = entity.WorldPos - initiator.WorldPos;
		
		var targetPos = entity.WorldPos + dir;
		if (!GameWorld.Get().IsBlocked(targetPos)) entity.WorldPos = targetPos;
	}

	public string Description => "Push";
}
