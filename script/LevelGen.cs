using Godot;
using System;
using System.ComponentModel;
using System.Linq;
using Godot.Collections;
using huntedrl.script;

[Tool]
public class LevelGen : Node
{
	public TileMap _wallMap;
	public TileMap _floorMap;
	
	private bool _run;

	private int[] _wallTiles;

	[Export]
	public bool Run
	{
		get => _run;
		set
		{
			_run = value;
			if (_run) Generate();
		}
	}

	public override void _Ready()
	{
		var cfg = new File();
		cfg.Open("res://resources/level_defs.json", File.ModeFlags.Read);
		var json = JSON.Parse(cfg.GetAsText());
		var rooms = json.Result;
		
		var _tiles = ResourceLoader.Load<TileSet>("res://resources/tiles/map_tileset.tres");
		_wallTiles = _tiles.GetTilesIds().OfType<int>()
			.Where(id => _tiles.TileGetName(id).StartsWith("wall"))
			.ToArray();

		GD.Print("Ready LevelGen");
	}

	public void Generate()
	{
		GD.Print("Generate Level");

		_wallMap = GetTree().EditedSceneRoot.GetNode<TileMap>("GameWorld/WallMap");
		_floorMap = GetTree().EditedSceneRoot.GetNode<TileMap>("GameWorld/FloorMap");

		_wallMap.Clear();

		var size = new Vector2(12, 8);
		
		foreach(var row in Enumerable.Range(0,3))
			foreach(var col in Enumerable.Range(0,3))
				WallRect(size * new Vector2(col,row), size);
		
		Run = false;
	}

	private void WallRect(Vector2 anchor, Vector2 span)
	{
		foreach (var x in Enumerable.Range((int)anchor.x, (int)span.x))
		{
			_wallMap.SetCell(x, (int)anchor.y, _wallTiles.FirstOrDefault());
			_wallMap.SetCell(x, (int)(anchor.y + span.y), _wallTiles.FirstOrDefault());
		}

		foreach (var y in Enumerable.Range((int)anchor.y, (int)span.y))
		{
			_wallMap.SetCell((int)anchor.x, y, _wallTiles.FirstOrDefault());
			_wallMap.SetCell((int)(anchor.x + span.x), y, _wallTiles.FirstOrDefault());
		}
		
		_wallMap.SetCell((int)(anchor.x + span.x), (int)(anchor.y + span.y), _wallTiles.FirstOrDefault());
	}
}
