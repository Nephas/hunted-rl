using Godot;
using System;
using System.Linq;
using huntedrl.script;

public class Decay : Node
{
	private int _counter;
	
	public override void _Ready()
	{
		Timer timer = GameWorld.Get().GetChildren().OfType<Timer>().FirstOrDefault();
		_counter = 5;
		
		timer.Connect("GameTurn", this, "Progress");
	}

	public void Progress()
	{
		_counter--;
		if (_counter<=0) this.GetEntity().QueueFree();
	}
}
