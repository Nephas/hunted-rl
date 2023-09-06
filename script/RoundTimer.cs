using Godot;
using System;

public class RoundTimer : Timer
{
	private RandomNumberGenerator _rng;

	public override void _Ready()
	{
		_rng = new RandomNumberGenerator();
		_rng.Randomize();
	}

	public void _on_Turn()
	{
		foreach (var a in GetTree().GetNodesInGroup("ai")){
			var node = a as Entity;
			node.WorldPos += new Vector2(_rng.RandiRange(-1, 1), _rng.RandiRange(-1, 1));
		}
	}
}
