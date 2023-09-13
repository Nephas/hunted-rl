using Godot;
using System;
using System.Linq;
using huntedrl.script;

public class BottomPanel : ColorRect
{
	private Label _actionsLabel;
	private Actor _pcActor;

	public override void _Ready()
	{
		_pcActor = this.GetPC().GetComponent<Actor>();
		_actionsLabel = this.GetChildByName<Label>("Actions");
		_actionsLabel.Modulate = Color.ColorN("orange");
		var _timer = GameWorld.Get().GetChildByName<Timer>("Timer");
		_timer.Connect("Tick", this, "OnTick");

	}

	private void OnTick()
	{
		_actionsLabel.Text = "Player Actions: " +  
							new string('>', _pcActor.Actions) + 
							new string('.', _pcActor.MaxActions - _pcActor.Actions);
	}
}
