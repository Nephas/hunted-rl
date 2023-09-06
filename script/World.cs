using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public class World : Node2D
{
	public static Vector2 TILESIZE = new Vector2(16,24);
	public static Vector2 WORLDSIZE = Vector2.Zero;

	public void Instantiate(string path, Vector2 pos){
		var pack = ResourceLoader.Load<PackedScene>(path);
		var obj = pack.Instance<Entity>();
		obj.WorldPos = pos;
		AddChild(obj);
	}

	public override void _Ready()
	{
		WORLDSIZE = GetViewport().Size / TILESIZE;

		Instantiate("res://prefab/pc.tscn", WORLDSIZE * 1/3);
		Instantiate("res://prefab/npc.tscn", WORLDSIZE * 2/3);

		foreach(var i in new List<int>{1,2,3,7,8,9}){
			Instantiate("res://prefab/wall.tscn", new Vector2(i,15));
		}
	}

	public override void _Process(float delta)
	{
	}
}
