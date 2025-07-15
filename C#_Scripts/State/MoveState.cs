using Godot;
using System;

public partial class MoveState : State
{
	[Export] public State IdleState { get; set; }
	[Export] public State FallState { get; set; }
	[Export] public State JumpState { get; set; }
	[Export] public State BackpackState { get; set; }

	private float timeSinceInput = 0f;

	public override void Enter()
	{
		Parent.PlayerAnimationTree.SetRunAnimation(true);
	}

    public override void Exit()
    {
		Parent.PlayerAnimationTree.SetRunAnimation(false);
    }


	public override State ProcessInput(InputEvent @event)
	{
		CheckNonStateInput();

		if (Input.IsActionJustPressed("jump") && Parent.IsOnFloor())
			return JumpState;

		if (Input.IsActionJustPressed("backpack"))
			return BackpackState;

		return null;
	}

	public override State ProcessPhysics(double delta)
	{
		float movement = Input.GetAxis("move-left", "move-right") * Parent.MoveSpeed;

		if (!Parent.IsOnFloor())
			return FallState;

		if (movement != 0)
		{
			Parent.Velocity = new Vector2(movement, Parent.Velocity.Y);
			timeSinceInput = 0f;
		}
		else
		{
			Parent.Velocity = new Vector2(Mathf.MoveToward(Parent.Velocity.X, 0, Parent.StopSpeed * (float) delta), Parent.Velocity.Y);

			if (Mathf.IsEqualApprox(Parent.Velocity.X, 0))
				timeSinceInput += (float) delta;
		}

		// if returned to idle state immediately the movement gets clunky
		if (timeSinceInput > 0.1)
			return IdleState;

		Parent.MoveAndSlide();

		return null;
	}
}
