using System.Linq;
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

	public T GetComponent<T>()
	{
		return GetChildren().OfType<T>().FirstOrDefault();
	}

	public bool IsInteractable()
	{
		return GetChildren().Cast<IInteractable>().Any();
	}
	
	public void InteractWith(Entity initiator)
	{
		var interactable = GetChildren().Cast<IInteractable>().FirstOrDefault(i => i != null);
		GD.Print($"Triggering Interaction '{interactable?.Description}'");
		interactable?.Interact(initiator);
	}
	
	public override void _Ready()
	{
	}
}

public interface IInteractable
{
	void Interact(Entity initiator);
	string Description { get; }
}
