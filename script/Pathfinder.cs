using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public class Pathfinder : Node
{
	private AStar2D _aStarMap;
	private IEnumerable<Vector2> _allPositions;

	public override void _Ready()
	{
		_aStarMap = new AStar2D();
		_allPositions = Enumerable.Range(0, (int)GameWorld.Worldsize.x)
			.SelectMany(x => Enumerable.Range(0, (int)GameWorld.Worldsize.y)
				.Select(y => new Vector2(x, y)));

		foreach (var pos in _allPositions)
			_aStarMap.AddPoint(posToid(pos), pos);

		foreach (var pos in _allPositions)
		{
			var down = pos + Vector2.Down;
			var right = pos + Vector2.Right;

			if (_aStarMap.HasPoint(posToid(down))) _aStarMap.ConnectPoints(posToid(pos), posToid(down));
			if (_aStarMap.HasPoint(posToid(right))) _aStarMap.ConnectPoints(posToid(pos), posToid(right));
		}

		UpdateAStar();
	}

	public void UpdateAStar(params Vector2[] positions)
	{
		foreach (var pos in positions)
			_aStarMap.SetPointDisabled(posToid(pos), IsBlocked(pos));
	}

	public void UpdateAll()
	{
		foreach (var pos in _allPositions)
			_aStarMap.SetPointDisabled(posToid(pos), IsBlocked(pos));
	}

	public bool AreConnected(Vector2 from, Vector2 to) => _aStarMap.ArePointsConnected(posToid(from), posToid(to));
	public IEnumerable<Vector2> GetPath(Vector2 from, Vector2 to) => _aStarMap.GetPointPath(posToid(from), posToid(to)).AsEnumerable();
	
	private Vector2 idToPos(int id) => new Vector2(id % 1000, id / 1000);
	private int posToid(Vector2 pos) => posToid((int)pos.x, (int)pos.y);
	
	private int posToid(int x, int y) => y * 1000 + x;

	private bool IsBlocked (Vector2 pos) => GameWorld.Get().IsBlocked(pos);
}
