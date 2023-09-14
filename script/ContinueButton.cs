using huntedrl.script;

using Godot;

public class ContinueButton : Button
{
	[Export]
	public string TargetScene = "main";

	public void OnPress(){ 
		this.LoadScene(TargetScene);
	}
	
	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_accept")) this.LoadScene(TargetScene);
	}
}
