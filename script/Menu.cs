using Godot;

public class Menu : Node2D
{
	private void _on_MenuButton()
	{
		GetTree().ChangeScene("res://main.tscn");
	}
}

