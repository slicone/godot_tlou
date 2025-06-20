using Godot;
using System;

public partial class State : Node
{
	[Export]
	public string AnimationName { get; set; }

	[Export]
	public Sprite2D Animation { get; set; }

	[Export]
	public float MoveSpeed { get; set; } = 300f;

	[Export]
	public float StopSpeed { get; set; } = 900f;

	protected float Gravity;

	public Player Parent { get; set; }

	public virtual void Enter()
	{
		Parent.AnimationPlayer.Play(AnimationName);
		if (Animation != null)
			Animation.Visible = true;
	}

	public virtual void Exit()
	{
		if (Animation != null)
			Animation.Visible = false;
	}

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
			Parent.TriggerInteract();

		if (Input.IsActionJustPressed("drop"))
			Parent.TriggerDrop();

		if (Input.IsActionJustPressed("attack"))
			Parent.TriggerAttack();
	}
}
