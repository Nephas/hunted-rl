using Godot;
using System.Collections.Generic;
using System.Linq;

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
		GetTree().CallGroup("ai", "TakeAIAction");
		
		if (_actors.Peek().Actions > 0) return;
		
		var next = _actors.Dequeue();
		_actors.Enqueue(next);
		_actors.First().Actions = 4;
	}
}
