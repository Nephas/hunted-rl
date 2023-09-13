using Godot;
using System;
using System.Linq;
using huntedrl.script;

public class ToolTip : ColorRect
{
	private Label _nameLabel; 
	private Label _actionLabel;
	
	private bool _show;
	private bool _lock;

	public bool Lock
	{
		get => _lock;
		set => MouseFilter = value ? MouseFilterEnum.Ignore : MouseFilterEnum.Pass;
	}
	
	public bool Show
	{
		get => _show;
		set
		{
			GetChild<Node2D>(0).Visible = value;
			SelfModulate = new Color(Colors.White, value ? 0.5F : 0F);
		}
	}

	public override void _Ready()
	{
		SelfModulate = Colors.Transparent;
		GetChild<Node2D>(0).Visible = false;
		_nameLabel = GetChild(0).GetChildren().OfType<Label>().FirstOrDefault(l => l.Name == "Name");
		_nameLabel.Text = Name;

		_actionLabel = GetChild(0).GetChildren().OfType<Label>().FirstOrDefault(l => l.Name == "Action");

		if (!GetParent<Entity>().IsInteractable())
		{
			_actionLabel.Visible = false;
			return;
		}
		
		_actionLabel.Text = $"[space]: {GetParent<Entity>().GetComponent<IInteractable>().Description}";

	}

	private void _on_ToolTip_mouse_entered() => Show = true;

	private void _on_ToolTip_mouse_exited() => Show = false;
}
