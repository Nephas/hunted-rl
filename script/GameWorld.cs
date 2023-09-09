using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Management.Instrumentation;
using Godot;

public class GameWorld : Node2D
{
	public static List<Vector2> CARDINALS = new List<Vector2>
		{ Vector2.Up, Vector2.Down, Vector2.Left, Vector2.Right };

	private static GameWorld _instance;
	public static Vector2 Tilesize = new Vector2(16,24);
	public static Vector2 Worldsize = Vector2.Zero;

	private TileMap _wallMap;
	private int[] _wallTiles;

	public static GameWorld Get() => _instance;

	public void Instantiate(string name, int x, int y)
	{
		Vector2 pos = new Vector2(x, y);
		var pack = ResourceLoader.Load<PackedScene>($"res://prefab/{name}.tscn");
		var obj = pack.Instance<Entity>();
		obj.WorldPos = pos;
		AddChild(obj);
	}
	
	public override void _EnterTree()
	{
		base._EnterTree();
		_instance = this;
		
		Worldsize = GetViewport().Size / Tilesize;

		/*
		var file = new File();
		file.Open("res://resources/level.tres", File.ModeFlags.Read);

		int j = 0;
		foreach (var line in file.GetAsText().Split("\n"))
		{
			int i = 0;
			foreach(var c in line)
			{
				Instantiate("floor", i, j);
				switch (c)
				{
					case '#': Instantiate("wall", i,j); break;
					case '@': Instantiate("pc", i, j); break;
					case '€': Instantiate("npc", i, j); break;
					case 'E': Instantiate("escape", i, j); break;
					case 'D': Instantiate("door", i, j); break;
				}
				i++;
			}
			j++;
		}
		*/

//		Instantiate("pc", 10, 10);
		
		_wallMap = GetChildren().OfType<TileMap>().FirstOrDefault(tm => tm.Name == "WallMap");
		var _tiles = ResourceLoader.Load<TileSet>("res://resources/tiles/map_tileset.tres");
		_wallTiles = _tiles.GetTilesIds().OfType<int>()
			.Where(id => _tiles.TileGetName(id).StartsWith("wall"))
			.ToArray();

		_wallMap.SetCell(1,1, _wallTiles[0]);
	}

	public bool IsBlocked(Vector2 pos)
	{
		var isWall = _wallTiles.Contains(_wallMap.GetCellv(pos));
		var isBlocked = GetTree().GetNodesInGroup("blocked")
			.OfType<Entity>().Any(e => e.WorldPos == pos);
		return isBlocked || isWall;
	}

	public IEnumerable<Entity> GetAllEntities() => GetTree().GetNodesInGroup("entity").OfType<Entity>();

	public IEnumerable<Entity> GetEntitiesAt(Vector2 pos) => GetAllEntities().Where(e => e.WorldPos == pos);

	public IEnumerable<Entity> GetNeighboringEntities(Vector2 pos) 
		=> CARDINALS.Select(dir => pos + dir)
			.SelectMany(neighPos => GetEntitiesAt(neighPos));
}
