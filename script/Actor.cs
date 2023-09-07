using Godot;
using System.Linq;
using huntedrl.script;

public class Actor : Label
{
	private int _actions;
	private AI _ai;
	private bool _alive;

	public int Actions
	{
		get => _actions;
		set => Text = GetActionBar(_actions = value);
	}

	public override void _Ready()
	{
		Actions = 0;
		_alive = true;
		_ai = this.GetEntity().GetComponent<AI>();
	}

	private string GetActionBar(int n)
	{
		var text = "";
		for (var i = 0; i < n; i++) text += ".";
		return text;
	}

	public bool CanMove(Vector2 dir)
	{
		var pos = this.GetEntity().WorldPos;
		return !World.Get().IsBlocked(pos + dir);
	}

	public void Move(Vector2 dir)
	{
		this.GetEntity().WorldPos += dir;
		Actions--;
	}
	
	public void TakeAIAction(){
		if (Actions <= 0) return;
		var dir = _ai.GetMoveDir();
		if (dir != Vector2.Zero)
			Move(dir);
		else
			Actions--;
	}
}
