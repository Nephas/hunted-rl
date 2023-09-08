using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Management.Instrumentation;
using Godot;

public class World : Node2D
{
	public static List<Vector2> CARDINALS = new List<Vector2>
		{ Vector2.Up, Vector2.Down, Vector2.Left, Vector2.Right };
	
	public static Vector2 Tilesize = new Vector2(16,24);
	public static Vector2 Worldsize = Vector2.Zero;
	private static World _instance;

	public static World Get() => _instance;

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
	}

	public bool IsBlocked(Vector2 pos)
	{
		return GetTree().GetNodesInGroup("blocked")
			.OfType<Entity>().Any(e => e.WorldPos == pos);
	}

	public IEnumerable<Entity> GetAllEntities() => GetChildren().OfType<Entity>();

	public IEnumerable<Entity> GetEntitiesAt(Vector2 pos) => GetAllEntities().Where(e => e.WorldPos == pos);

	public IEnumerable<Entity> GetNeighboringEntities(Vector2 pos) 
		=> CARDINALS.Select(dir => pos + dir)
			.SelectMany(neighPos => GetEntitiesAt(neighPos));
}
