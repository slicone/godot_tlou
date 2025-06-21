using Godot;
using System;

public partial class JumpState : State
{
	[Export] 
	public State IdleState { get; set; }
	[Export] 
	public State MoveState { get; set; }
	[Export] 
	public State FallState { get; set; }

	public override State ProcessInput(InputEvent @event)
	{
		CheckNonStateInput();
		return null;
	}

	public override State ProcessPhysics(double delta)
	{
		if (Parent == null)
			return null;

		// Apply vertical jump velocity
		Parent.Velocity = new Vector2(Parent.Velocity.X, Parent.JumpVelocity);

		if (!Parent.IsOnFloor())
			return FallState;

		float input = Input.GetAxis("move-left", "move-right");
		float movement = input * Parent.MoveSpeed;

		if (movement != 0)
		{
			if (Animation != null)
				Animation.FlipH = movement < 0;
		}

		Parent.Velocity = new Vector2(movement, Parent.Velocity.Y);
		Parent.MoveAndSlide();

		if (Parent.IsOnFloor())
		{
			if (movement != 0)
				return MoveState;
			return IdleState;
		}

		return null;
	}
}