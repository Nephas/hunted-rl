using System.Dynamic;
using System.Linq;
using Godot;

public class World : Node2D
{
	public static Vector2 Tilesize = new Vector2(16,24);
	public static Vector2 Worldsize = Vector2.Zero;
	private static World _instance;

	public static World Get() => _instance;
	
	public void Instantiate(string path, Vector2 pos){
		var pack = ResourceLoader.Load<PackedScene>(path);
		var obj = pack.Instance<Entity>();
		obj.WorldPos = pos;
		AddChild(obj);
	}

	public override void _EnterTree()
	{
		base._EnterTree();
		_instance = this;
		
		Worldsize = GetViewport().Size / Tilesize;

		Instantiate("res://prefab/pc.tscn", new Vector2(6,6));
		Instantiate("res://prefab/npc.tscn", new Vector2(14,6));
		Instantiate("res://prefab/npc.tscn", new Vector2(15,7));

		InstantiateWalls();
	}

	private void InstantiateWalls()
	{
		var res = ResourceLoader.Load<StreamTexture>("res://resources/level_map.png", "Image");
		var img = res.GetData();
		img.Lock();
		for (int i =0; i < img.GetWidth(); i++)
			for (int j =0; j < img.GetHeight(); j++)
				if (img.GetPixel(i,j).v < 0.5) Instantiate("res://prefab/wall.tscn", new Vector2(i,j));
	}

	public bool IsBlocked(Vector2 pos)
	{
		return GetTree().GetNodesInGroup("blocked")
			.OfType<Entity>().Any(e => e.WorldPos == pos);
	}

	public void OnPCDeath()
	{
		
	}
}
