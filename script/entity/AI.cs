using Godot;
using System.Collections.Generic;
using System.Linq;
using huntedrl.script;

public class AI : Node
{
	private RandomNumberGenerator _rng;
	private Entity _pc;
	private Actor _actor;
	private Queue<Vector2> _path;

	public override void _Ready()
	{
		_pc = this.GetPC();
		_actor = this.GetEntity().GetComponent<Actor>();
		_rng = new RandomNumberGenerator();
	}

	public Vector2 GetMoveDir()
	{
		if (_path.Count <= 1) return Vector2.Zero;
		var current = _path.Dequeue();
		var next = _path.Peek();
		return next - current;
	}

	public bool NextToPC()
	{
		return (this.GetEntity().WorldPos - _pc.WorldPos).Length() == 1;
	}

	public void InitiateTurn()
	{
		SpawnBlip();

		var target = GameWorld.CARDINALS
			.Select(dir => this.GetPC().WorldPos + dir)
			.FirstOrDefault(pos => !GameWorld.Get().IsBlocked(pos));
		
		GameWorld.Get().Pathfinder.UpdateAll();
		var path = GameWorld.Get().Pathfinder.GetPath(this.GetEntity().WorldPos, target);
		_path = new Queue<Vector2>(path);
	}
	
	public void TakeAction(){
		if (NextToPC()) 
			_actor.Attack(_pc.GetComponent<Actor>());
		
		var dir = GetMoveDir();
		if (dir != Vector2.Zero && _actor.CanMove(dir))
		{
			_actor.Move(dir);
		}
		else
			_actor.Actions--;
	}

	public void SpawnBlip()
	{
		if (this.GetPC().HasLos(this.GetEntity())) return;
		var dir = GameWorld.CARDINALS.OrderBy(_ => _rng.Randi()).FirstOrDefault();
		GameWorld.Get().Instantiate("blip", this.GetEntity().WorldPos + dir);
	}
}
