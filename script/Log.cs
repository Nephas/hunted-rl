using Godot;
using System;

public class Log : Label
{
	private static string content = "";
	
	public override void _Ready()
	{
		Text = content;
	}

	public static void AddLine(string text)
	{
		content = $"- {text}\n{content}";
	}
}
