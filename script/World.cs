using Godot;

public class World : Node2D
{
	public static Vector2 Tilesize = new Vector2(16,24);
	public static Vector2 Worldsize = Vector2.Zero;

	public void Instantiate(string path, Vector2 pos){
		var pack = ResourceLoader.Load<PackedScene>(path);
		var obj = pack.Instance<Entity>();
		obj.WorldPos = pos;
		AddChild(obj);
	}

	public override void _EnterTree()
	{
		base._EnterTree();
		
		Worldsize = GetViewport().Size / Tilesize;

		Instantiate("res://prefab/pc.tscn", Worldsize * 1/4);
		Instantiate("res://prefab/npc.tscn", Worldsize * 2/4);
		Instantiate("res://prefab/npc.tscn", Worldsize * 3/4);

		var rng = new RandomNumberGenerator();
		rng.Randomize();
		
		for(int i = 0; i<5; i++){
			Instantiate("res://prefab/wall.tscn", new Vector2(rng.RandfRange(0, Worldsize.x),rng.RandfRange(0, Worldsize.y)));
		}
	}
}
