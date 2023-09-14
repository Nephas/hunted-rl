using Godot;
using System;
using System.Linq;
using huntedrl.script;

public class PC : Node
{
	private Actor _actor;
	private ToolTip _focus;
	
	public override void _Ready()
	{
		_actor = this.GetEntity().GetComponent<Actor>();

		var _timer = GameWorld.Get().GetChildByName<Timer>("Timer");
		_timer.Connect("Tick", this, "UpdateFoV");
		_actor.Connect("OnDeath", this, "OnPCDeath");
	}

	private void OnPCDeath() => this.LoadScene("failure");

	public void UpdateHighlights()
	{
		if (_focus != null)
		{
			_focus.Show = false;
			_focus.Lock = false;
		}
		
		var interactable = GameWorld.Get()
			.GetNeighboringEntities(this.GetEntity().WorldPos)
			.FirstOrDefault(e => e.IsInteractable());

		if (interactable == null) return;
		_focus = interactable.GetComponent<ToolTip>();
		_focus.Show = true;
		_focus.Lock = true;
	}
	
	public void UpdateFoV()
	{
		var space = GameWorld.Get().GetWorld2d().DirectSpaceState;
		var fovEntities = GameWorld.Get().GetAllEntities()
			.Where(e => e.IsInGroup("fov"));
		var pc = GameWorld.Get().GetPC();

		foreach (var target in fovEntities)
			target.Show = pc.HasLos(target);
		
		foreach (var target in GetTree().GetNodesInGroup("blip").OfType<Entity>())
			if (pc.HasLos(target)) target.QueueFree();
	}
}
