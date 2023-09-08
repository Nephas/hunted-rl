using Godot;
using System.Collections.Generic;
using System.Linq;
using huntedrl.script;

public class RoundTimer : Timer
{
	private RandomNumberGenerator _rng;
	private Queue<Actor> _actors = new Queue<Actor>();

	public override void _Ready()
	{
		foreach (var actor in GetTree().GetNodesInGroup("actor"))
			_actors.Enqueue(actor as Actor);

		_rng = new RandomNumberGenerator();
		_rng.Randomize();
	}

	public void _on_Turn()
	{
		var next = _actors.Peek();
		if (next.GetEntity().IsInGroup("pc") && next.Actions > 0) return;
		if (next.Actions > 0) next.GetEntity().GetComponent<AI>().TakeAIAction();
		else QueueNextActor();
	}

	private void QueueNextActor()
	{
		var next = _actors.Dequeue();
		_actors.Enqueue(next);
		_actors.First().Actions = 4;
		GD.Print("next Actor:" + next.GetEntity().Name);
	}
}
