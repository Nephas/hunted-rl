using Godot;
using System;
using System.Linq;

public class ToolTip : Label
{
	public override void _Ready()
	{
		Text = this.GetParent().Name;
		this.Visible = false;
	}

	private void _on_ToolTip_mouse_entered()
	{
		this.Visible = true;
	}

	private void _on_ToolTip_mouse_exited()
	{
		this.Visible = false;
	}
}
