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
	public static Vector2 Tilesize =  Vector2.Zero;
	public static Vector2 Worldsize = Vector2.Zero;

	private TileMap _wallMap;
	private int[] _wallTiles;
	public Pathfinder Pathfinder;
	
	public static GameWorld Get() => _instance;

	public void Instantiate(string name, Vector2 pos)
	{
		var pack = ResourceLoader.Load<PackedScene>($"res://prefab/{name}.tscn");
		var obj = pack.Instance<Entity>();
		obj.WorldPos = pos;
		GetChildren().OfType<Node>().FirstOrDefault(n => n.Name == "Entities")?.AddChild(obj);
	}
	
	public override void _EnterTree()
	{
		base._EnterTree();
		_instance = this;

		_wallMap = GetChildren().OfType<TileMap>().FirstOrDefault(tm => tm.Name == "WallMap");
		var _tiles = ResourceLoader.Load<TileSet>("res://resources/tiles/map_tileset.tres");
		_wallTiles = _tiles.GetTilesIds().OfType<int>()
			.Where(id => _tiles.TileGetName(id).StartsWith("wall"))
			.ToArray();

		Pathfinder = GetChildren().OfType<Pathfinder>().FirstOrDefault();
		
		var cells = _wallMap.GetUsedCells().OfType<Vector2>();
		Worldsize = new Vector2(cells.Max(v => v.x),cells.Max(v => v.y));
		Tilesize = _wallMap.CellSize;
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
