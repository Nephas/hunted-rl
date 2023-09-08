using Godot;
using System;

public class Log : Label
{
	private static Label _instance;
	
	public override void _Ready()
	{
		_instance = this;
	}

	public static void AddLine(string text)
	{
		_instance.Text = $"- {text}\n{_instance.Text}";
	}
}
