using Godot;
using System;

public class World : Node2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	private Sprite _pc;
	private RandomNumberGenerator _rng;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var pack = ResourceLoader.Load<PackedScene>("res://sprite/pc.tscn");
		_pc = pack.Instance<Sprite>();
		_pc.Position = new Vector2(100, 100);
		
		AddChild(_pc);

		_rng = new RandomNumberGenerator();
		_rng.Randomize();
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		Console.WriteLine("YO");

		_pc.Position = _pc.Position + GetRandVec();
	}

	public Vector2 GetRandVec(){
		return new Vector2(_rng.RandfRange(-1f, 1f), _rng.RandfRange(-1f, 1f));
	}
}
