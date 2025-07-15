using Godot;
using System;

public partial class State : Node
{
	[Export]
	public string AnimationName { get; set; }

	[Export]
	public Sprite2D Animation { get; set; }

	public Player Parent { get; set; }

	public virtual void Enter() {}

	public virtual void Exit() {}

	public virtual State ProcessInput(InputEvent inputEvent)
	{
		return null;
	}

	public virtual State ProcessFrame(double delta)
	{
		return null;
	}

	public virtual State ProcessPhysics(double delta)
	{
		return null;
	}

	protected void CheckNonStateInput()
	{
		if (Input.IsActionJustPressed("interact"))
		{
			Parent.TriggerInteract();
		}

		if (Input.IsActionJustPressed("drop"))
		{
			Parent.TriggerDrop();
		}

		if (Input.IsActionJustPressed("attack"))
		{
			Parent.TriggerAttack();
		}
	}
}
