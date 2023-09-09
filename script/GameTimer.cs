using Godot;
using System.Collections.Generic;
using System.Linq;
using huntedrl.script;

public class GameTimer : Timer
{
	private RandomNumberGenerator _rng;
	private Queue<Actor> _actors = new Queue<Actor>();

	[Signal]
	public delegate void GameTick();
	
	public override void _Ready()
	{
		foreach (var actor in GetTree().GetNodesInGroup("actor"))
			_actors.Enqueue(actor as Actor);

		_rng = new RandomNumberGenerator();
		_rng.Randomize();
	}

	public void OnTick()
	{
		EmitSignal("GameTick");
		var next = _actors.Peek();
		if (next.GetEntity().IsInGroup("pc") && next.Actions > 0) return;
		if (next.Actions > 0) next.GetEntity().GetComponent<AI>().TakeAIAction();
		else QueueNextActor();
		
//		GameWorld.Get().GetPC().GetComponent<PC>().UpdateFoV();
	}

	private void QueueNextActor()
	{
		var next = _actors.Dequeue();
		_actors.Enqueue(next);
		_actors.First().Actions = 4;
		Log.AddLine($"Begin {next.GetEntity().Name}'s turn");
	}
}
