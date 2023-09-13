using Godot;
using huntedrl.script;

public class Door : Node, IInteractable
{
	private AnimatedSprite _animation;
	private LightOccluder2D _occluder;
	private bool _open;
	
	public bool Open
	{
		get => _open;
		set
		{
			_open = value;
			if (_open) {
				_animation.Frame = 1;
				_occluder.LightMask = 20;
				this.GetEntity().RemoveFromGroup("blocked");
			}
			else
			{
				_animation.Frame = 0;
				_occluder.LightMask = 1;
				this.GetEntity().AddToGroup("blocked");
			}
		}
	} 
	
	public override void _Ready()
	{
		_animation = this.GetEntity().GetComponent<AnimatedSprite>();
		_occluder = this.GetEntity().GetComponent<LightOccluder2D>();
		Open = false;
	}

	public void Interact(Entity initiator)
	{
		Open = !Open;
	}

	[Export]
	public string Description => "Open/Close";
}
