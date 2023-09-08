using Godot;
using System.Linq;
using huntedrl.script;

public class Actor : Label
{
	private int _actions;
	private bool _alive;

	[Signal]
	public delegate void OnDeath();

	public bool Alive
	{
		get => _alive;
		set
		{
			if (value == false) EmitSignal("OnDeath");
			_alive = value;
		}
	}
	
	public int Actions
	{
		get => _actions;
		set => Text = GetActionBar(_actions = value);
	}

	public override void _Ready()
	{
		Actions = 0;
		_alive = true;
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

	public void Attack(Actor other)
	{
		other.Alive = false;
	}
	
	public void Move(Vector2 dir)
	{
		this.GetEntity().WorldPos += dir;
		Actions--;
	}
}
