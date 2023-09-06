using Godot;

public class Entity : Node2D
{
	private Vector2 _worldPos;

	public Vector2 WorldPos {
		get => _worldPos;
		set => SetWorldPos(value);
	}

	private void SetWorldPos(Vector2 worldPos){
		_worldPos = worldPos.Floor();
		Position = World.Tilesize * worldPos;
	}

	public override void _Ready()
	{
	}
}
