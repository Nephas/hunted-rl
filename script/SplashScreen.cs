using huntedrl.script;

using Godot;

public class SplashScreen : Control
{
	public void _onContinueButton() => Continue();
	
	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_accept")) Continue();
	}

	private void Continue()
	{
		GD.Print("Continue Button Pressed");
		if (IsInGroup("intro")) this.LoadScene("main");
		
		if (IsInGroup("success") || IsInGroup("failure")) this.LoadScene("intro");
	}
}
