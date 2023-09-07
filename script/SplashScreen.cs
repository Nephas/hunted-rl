using Godot;

public class SplashScreen : Node2D
{
	private void _on_MenuButton() => Continue();

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_accept")) Continue();
	}

	private void Continue()
	{
		if (IsInGroup("intro-screen"))
			GetTree().ChangeScene("res://scenes/main.tscn");
		
		if (IsInGroup("death-screen"))
			GetTree().ChangeScene("res://scenes/intro.tscn");
	}
}

