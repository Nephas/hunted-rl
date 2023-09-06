using Godot;
using System.Linq;

public class Actor : Label
{
	private int _actions;
	private AI _ai;

	public int Actions
	{
		get => _actions;
		set => Text = GetActionBar(_actions = value);
	}

	public override void _Ready()
	{
		Actions = 0;
		_ai = GetParent().GetChildren().OfType<AI>().FirstOrDefault();
	}

	private string GetActionBar(int n)
	{
		var text = "";
		for (var i = 0; i < n; i++) text += ".";
		return text;
	}
	
	public void TakeAIAction(){
		if (Actions <= 0) return;
		_ai.Move();
		Actions--;
	}
}
