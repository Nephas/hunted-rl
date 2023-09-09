using System;
using System.Collections.Generic;
using Godot;
using System.Linq;
using huntedrl.script;

public class Input : Node
{
	private bool _cooldown;
	
	public void ResetCooldown()
	{
		_cooldown = false;
	}
	
	public override void _Input(InputEvent @event)
	{
		if (GetPCActor().Actions <= 0 || _cooldown) return;

		foreach (var mapping in ActionMap)
			if (@event.IsActionPressed(mapping.Key, true)) {
				if (mapping.Value.Invoke()) {
					Log.AddLine($"Player Action {mapping.Key}");
					_cooldown = true;
				}
			}
	}

	private static Actor GetPCActor() => GameWorld.Get().GetPC().GetComponent<Actor>();
	
	private Dictionary<string, Func<bool>> ActionMap = new Dictionary<string, Func<bool>>
	{
		{ "up", () => GetPCActor().TryMove(Vector2.Up) },
		{ "down", () => GetPCActor().TryMove(Vector2.Down) },
		{ "left", () => GetPCActor().TryMove(Vector2.Left) },
		{ "right", () => GetPCActor().TryMove(Vector2.Right) },
		{ "ui_accept", () => GetPCActor().TryContextInteract() },
	};
}
