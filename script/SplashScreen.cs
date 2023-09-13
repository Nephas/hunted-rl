using Godot;

public class SplashScreen : Control
{
	private void _on_MenuButton() => Continue();
	
	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_accept")) Continue();
	}

	private void Continue()
	{
		if (IsInGroup("intro"))
			GetTree().ChangeScene("res://scenes/main.tscn");
		
		if (IsInGroup("success") || IsInGroup("failure"))
			GetTree().ChangeScene("res://scenes/intro.tscn");
	}
}

