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
		_actor = _pc.GetChildren().OfType<Actor>().FirstOrDefault();
	}

	public override void _Input(InputEvent @event)
	{
		if (_actor.Actions <= 0) return;

		var move = Vector2.Zero;
		if (@event.IsActionPressed("up"))  move = Vector2.Up;
		if (@event.IsActionPressed("down")) move += Vector2.Down;
		if (@event.IsActionPressed("left")) move += Vector2.Left;
		if (@event.IsActionPressed("right")) move += Vector2.Right;

		if (move != Vector2.Zero)
		{
			_pc.WorldPos += move;
			_actor.Actions--;
		}
	}
}
