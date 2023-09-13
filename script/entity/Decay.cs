using Godot;
using System;
using System.Linq;
using huntedrl.script;

public class Decay : Node
{
	private int _counter;
	
	[Export]
	private int Turns = 5;
	
	public override void _Ready()
	{
		Timer timer = GameWorld.Get().GetChildByName<Timer>("Timer");
		this._counter = Turns;
		
		timer.Connect("Turn", this, "Progress");
	}

	public void Progress()
	{
		_counter--;
		if (_counter <= 0) this.GetEntity().QueueFree();
	}
}
