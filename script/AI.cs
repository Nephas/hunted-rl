using Godot;
using System.Collections.Generic;
using System.Linq;
using huntedrl.script;

public class AI : Node
{
	private RandomNumberGenerator _rng;
	private Entity _pc;
	private Actor _actor;
	
	public override void _Ready()
	{
		_pc = this.GetPC();
		_actor = this.GetEntity().GetComponent<Actor>();
		_rng = new RandomNumberGenerator();
	}

	public Vector2 GetMoveDir()
	{
		var pos = this.GetEntity().WorldPos;
		return World.CARDINALS
			.Where(dir => !World.Get().IsBlocked(pos + dir))
			.Append(Vector2.Zero)
			.OrderBy(dir => _pc.WorldPos.DistanceSquaredTo(pos + dir))
			.FirstOrDefault();
	}

	public bool NextToPC()
	{
		return (this.GetEntity().WorldPos - _pc.WorldPos).Length() == 1;
	}
	
	public void TakeAIAction(){
		if (NextToPC()) 
			_actor.Attack(_pc.GetComponent<Actor>());
		
		var dir = GetMoveDir();
		if (dir != Vector2.Zero)
			_actor.Move(dir);
		else
			_actor.Actions--;
	}
}
