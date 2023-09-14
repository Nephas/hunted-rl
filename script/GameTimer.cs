using Godot;
using System.Collections.Generic;
using System.Linq;
using huntedrl.script;

public class GameTimer : Timer
{
	private RandomNumberGenerator _rng;
	private Queue<Actor> _actors = new Queue<Actor>();

	private int _tick = 0; 
	private int _turn = 0; 
	
	[Signal]
	public delegate void Tick();
	
	[Signal]
	public delegate void Turn();
	
	public override void _Ready()
	{
		foreach (var actor in GetTree().GetNodesInGroup("actor"))
			_actors.Enqueue(actor as Actor);

		_rng = new RandomNumberGenerator();
		_rng.Randomize();
	}

	public void OnTick()
	{
		_tick++;
		EmitSignal("Tick");
		
		var next = _actors.Peek();
		if (next.GetEntity().IsInGroup("pc") && next.Actions > 0) return;
		if (next.Actions > 0) next.GetEntity().GetComponent<AI>().TakeAction();
		else QueueNextActor();
	}

	private void QueueNextActor()
	{
		var last = _actors.Dequeue();
		_actors.Enqueue(last);
		_actors.Peek().Actions = _actors.Peek().MaxActions;
		
		if (_actors.Peek().GetEntity().IsInGroup("pc")) OnTurn();
		else _actors.Peek().GetEntity().GetComponent<AI>().InitiateTurn();

		Log.AddLine($"Begin {_actors.Peek().GetEntity().Name}'s turn");
	}

	private void OnTurn()
	{
		_turn++;
		EmitSignal("Turn");
	}
}
