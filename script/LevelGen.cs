using Godot;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Godot.Collections;
using huntedrl.script;

public static class DiscreteRect
{
	public static Vector2 Floor(this Vector2 v) => new Vector2(Mathf.Floor(v.x), Mathf.Floor(v.x));

	public static IEnumerable<Vector2> GetAll(this Rect2 rect) =>
		from x in Enumerable.Range((int)rect.Position.x, 1 + (int)rect.Size.x) 
		from y in Enumerable.Range((int)rect.Position.y, 1 + (int)rect.Size.y) 
		select new Vector2(x, y);

	public static IEnumerable<Vector2> GetBoundary(this Rect2 rect) => 
		rect.GetAll().Where(pos =>
		pos.x == rect.Position.x || pos.x == rect.End.Floor().x ||
		pos.y == rect.Position.y || pos.y == rect.End.Floor().y
	).Distinct().AsEnumerable();
}

[Tool]
public class LevelGen : Node
{
	private enum Layout {BSP, Corridor, Empty}	

	private class Section
	{
		public static int MaxHeight = 64;
		
		public Rect2 Bounds;
		public string Purpose;
		public List<Vector2> Passages = new List<Vector2>();
		public List<Vector2> Doors = new List<Vector2>();
		public List<Vector2> Vents = new List<Vector2>();
		public List<Rect2> Rooms = new List<Rect2>();
		
		public Section(int x, int width, int heigth, string purpose, Layout layout = Layout.Empty)
		{
			Bounds = new Rect2(x, (MaxHeight - heigth) / 2, width, heigth);
			
			var entrance = new Vector2(x, Bounds.GetCenter().Floor().y);
			Passages.Add(entrance);
			Doors.Add(entrance);
			
			Purpose = purpose;

			if (layout == Layout.Corridor) BuildCorridorSection((int)GD.RandRange(4,7));
			if (layout == Layout.BSP) BuildBSPSection(4);
		}

		private void BuildCorridorSection(int roomWidth)
		{
			var corridor = new Rect2(new Vector2(Bounds.Position.x, Bounds.GetCenter().y - 2), new Vector2( Bounds.Size.x, 4));
			Rooms.Add(corridor);

			var upperRoom = new Rect2(
				Bounds.Position,
				new Vector2(roomWidth, corridor.Position.y - Bounds.Position.y));
					
			while (upperRoom.Position.x + roomWidth <= Bounds.End.x)
			{
				Rooms.Add(upperRoom);
				Passages.Add(new Vector2(upperRoom.GetCenter().Floor().x, upperRoom.End.y));
				upperRoom.Position = new Vector2(upperRoom.Position.x + roomWidth, upperRoom.Position.y);
			}

			var lowerRoom = new Rect2(
				new Vector2(Bounds.Position.x, corridor.End.y),
				new Vector2(roomWidth, Bounds.End.y - corridor.End.y));
					
			while (lowerRoom.Position.x + roomWidth <= Bounds.End.x)
			{
				Rooms.Add(lowerRoom);
				Passages.Add(new Vector2(lowerRoom.GetCenter().Floor().x, lowerRoom.Position.y));
				lowerRoom.Position = new Vector2(lowerRoom.Position.x + roomWidth, lowerRoom.Position.y);
			}
		}

		private void BuildBSPSection(int depth)
		{
			GD.Print("running BSP");
			Rooms = BSPSplit(Bounds, depth).ToList();
			GD.Print($"Depth: {depth}; Rooms: {Rooms.Count}");
		}

		private IEnumerable<Rect2> BSPSplit(Rect2 rect, int depth)
		{
			if (depth == 0) return new List<Rect2> { rect };
			
			var vertical = rect.Size.x > rect.Size.y;
			Rect2 rectA;
			Rect2 rectB;
			
			if (vertical)
			{
				int left = (int)(rect.Size.x * GD.RandRange(0.33, 0.66));
				int right = (int)(rect.Size.x - left);
				if (right < 4 || left < 4) return new List<Rect2> { rect };
				rectA = new Rect2(rect.Position, new Vector2(left, rect.Size.y));
				rectB = new Rect2(
					new Vector2(rect.Position.x + left, rect.Position.y),
					new Vector2(right, rect.Size.y));
			}
			else
			{
				int upper = (int)(rect.Size.y * GD.RandRange(0.33, 0.66));
				int lower = (int)(rect.Size.y - upper);
				if (upper < 4 || lower < 4) return new List<Rect2> { rect };
				rectA = new Rect2(rect.Position, new Vector2(rect.Size.x, upper));
				rectB = new Rect2(
					new Vector2(rect.Position.x, rect.Position.y + upper),
					new Vector2(rect.Size.x, lower));
			}

			return BSPSplit(rectA, depth - 1).Concat(BSPSplit(rectB, depth - 1));
		}
		
		public IEnumerable<Vector2> GetAllWalls()
		{
			var allRects = new List<Rect2>{Bounds}.Union(Rooms);
			foreach (var rect in allRects)
				foreach (var pos in rect.GetBoundary()) yield return pos;
		}
	}
	
	public TileMap _wallMap;
	public TileMap _floorMap;
	
	private bool _run;

	private int[] _wallTiles;
	private int[] _floorTiles;
	private Node _entities;

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

		GD.Print(_entities);
		
		var _tiles = ResourceLoader.Load<TileSet>("res://resources/tiles/map_tileset.tres");
		_wallTiles = _tiles.GetTilesIds().OfType<int>()
			.Where(id => _tiles.TileGetName(id).StartsWith("wall"))
			.ToArray();

		_floorTiles = _tiles.GetTilesIds().OfType<int>()
			.Where(id => _tiles.TileGetName(id).StartsWith("floor"))
			.ToArray();

		GD.Print("Ready LevelGen");
	}

	public void Generate()
	{
		GD.Print("Generate Level");

		_wallMap = GetTree().EditedSceneRoot.GetNode<TileMap>("GameWorld/WallMap");
		_floorMap = GetTree().EditedSceneRoot.GetNode<TileMap>("GameWorld/FloorMap");
		_entities = GetTree().EditedSceneRoot.GetNode<Node2D>("GameWorld/Entities");

		_wallMap.Clear();
		_floorMap.Clear();

		List<(string, Layout, int, int)> levelDef = new List<(string, Layout, int, int)>
		{
			("Engineering", Layout.BSP, 16, 24),
			("Cargo", Layout.Corridor, 32, 16),
			("Lab", Layout.BSP, 24, 24),
			("Hab", Layout.Corridor, 32, 16),
			("Command", Layout.Empty, 16, 16)
		};

		var sections = new List<Section>();
		Section previous = null;
		foreach (var def in levelDef)
		{
			int x = previous == null ? 0 : (int)previous.Bounds.End.x;
			int width = (int)(def.Item3 * GD.RandRange(0.66, 1.33));
			int heigth = (int)(def.Item4 * GD.RandRange(0.66, 1.33));
			sections.Add(new Section(x, width, heigth, def.Item1, def.Item2));
			previous = sections.Last();
		}
		
		foreach (var section in sections)
		{
			GD.Print("Initializing Walls");
			foreach (var pos in section.GetAllWalls())
				_wallMap.SetCell((int)pos.x, (int)pos.y, _wallTiles.FirstOrDefault());

			GD.Print("Initializing Floors");
			foreach (var pos in section.Bounds.GetAll())
				_floorMap.SetCell((int)pos.x, (int)pos.y, _floorTiles.FirstOrDefault());

			GD.Print("Initializing Passages");
			foreach (var pos in section.Passages)
			{
				_wallMap.SetCell((int)pos.x, (int) pos.y, -1);
			}
			//
			// GD.Print("Initializing Doors");
			// foreach (var pos in section.Doors)
			// {
			// 	Instantiate( "door", pos);
			// }
		}
		
		Run = false;
	}
	
	public void Instantiate(string name, Vector2 pos)
	{
		var pack = ResourceLoader.Load<PackedScene>($"res://prefab/{name}.tscn");
		var obj = pack.Instance<Entity>();
		obj.WorldPos = pos;
		_entities.AddChild(obj);
	}
}
