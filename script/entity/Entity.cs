using System.Linq;
using Godot;

public class Entity : Node2D
{
	private Vector2 _worldPos;
	private bool _show;

	public Vector2 WorldPos {
		get => _worldPos;
		set => SetWorldPos(value);
	}

	public bool Show { 
		get => _show ;
		set
		{
			var tooltip = GetComponent<ToolTip>();
			Visible = value;
			tooltip.Lock = !value;
		}}
	
	private void SetWorldPos(Vector2 worldPos){
		_worldPos = worldPos.Floor();
		Position = GameWorld.Tilesize * (worldPos + new Vector2(0.5f,0.5f));
	}

	public T GetComponent<T>()
	{
		return GetChildren().OfType<T>().FirstOrDefault();
	}

	public bool IsInteractable()
	{
		return GetChildren().OfType<IInteractable>().Any();
	}
	
	public void InteractWith(Entity initiator)
	{
		var interactable = GetChildren().OfType<IInteractable>().FirstOrDefault(i => i != null);
		Log.AddLine($"Interaction: '{interactable?.Description}'");
		interactable?.Interact(initiator);
	}
	
	public override void _Ready()
	{
		_worldPos = (Position / GameWorld.Tilesize).Floor();
		AddToGroup("entity");
	}
}

public interface IInteractable
{
	void Interact(Entity initiator);
	
	string Description { get; }
}
