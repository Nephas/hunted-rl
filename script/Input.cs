using Godot;
using System.Linq;
using huntedrl.script;

public class Input : Node
{
	private Entity _pc;
	private Actor _actor;
	
	public override void _Ready()
	{
		_pc = this.GetPC();
		_actor = _pc.GetComponent<Actor>();
	}

	public override void _Input(InputEvent @event)
	{
		if (_actor.Actions <= 0) return;

		var dir = Vector2.Zero;
		if (@event.IsActionPressed("up"))  dir = Vector2.Up;
		if (@event.IsActionPressed("down")) dir += Vector2.Down;
		if (@event.IsActionPressed("left")) dir += Vector2.Left;
		if (@event.IsActionPressed("right")) dir += Vector2.Right;

		if (dir != Vector2.Zero && _actor.CanMove(dir))
			_actor.Move(dir);
		
		if (@event.IsActionPressed("ui_accept")) _pc.GetComponent<PC>().ContextInteract();
	}
}
