using Godot;
using System.Collections.Generic;
using System.Linq;
using huntedrl.script;

public class AI : Node
{
	private RandomNumberGenerator _rng;
	private Entity _pc;

	private static List<Vector2> CARDINALS = new List<Vector2>
		{ Vector2.Up, Vector2.Down, Vector2.Left, Vector2.Right };

	public override void _Ready()
	{
		_pc = this.GetPC();
		_rng = new RandomNumberGenerator();
	}

	public Vector2 GetMoveDir()
	{
		var pos = this.GetEntity().WorldPos;
		return CARDINALS
			.Where(dir => !World.Get().IsBlocked(pos + dir))
			.Append(Vector2.Zero)
			.OrderBy(dir => _pc.WorldPos.DistanceSquaredTo(pos + dir))
			.FirstOrDefault();
	}
}
