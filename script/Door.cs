using Godot;
using huntedrl.script;

public class Door : Node, IInteractable
{
	private AnimatedSprite _animation;
	private bool _open;
	
	public bool Open
	{
		get => _open;
		set
		{
			_open = value;
			if (_open) {
				_animation.Frame = 1;
				this.GetEntity().RemoveFromGroup("blocked");
			}
			else
			{
				_animation.Frame = 0;
				this.GetEntity().AddToGroup("blocked");
			}
		}
	} 
	
	public override void _Ready()
	{
		_animation = this.GetEntity().GetComponent<AnimatedSprite>();
		Open = false;
	}

	public void Interact(Entity initiator)
	{
		Open = !Open;
	}

	public string Description => "Open/Close";
}
