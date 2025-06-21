using Godot;
using System;

public partial class IdleState : State
{
	[Export] 
	public State FallState { get; set; }
	[Export] 
	public State JumpState { get; set; }
	[Export] 
	public State MoveState { get; set; }

	public override void Enter()
	{
		base.Enter();
	}

	public override State ProcessInput(InputEvent @event)
	{
		CheckNonStateInput();

		if (Input.IsActionJustPressed("jump") && Parent.IsOnFloor())
			return JumpState;

		if (Input.IsActionJustPressed("move-left") || Input.IsActionJustPressed("move-right"))
			return MoveState;

		return null;
	}

	public override State ProcessPhysics(double delta)
	{
		if (!Parent.IsOnFloor())
			return FallState;

		float input = Input.GetAxis("move-left", "move-right");
		float movement = input * Parent.MoveSpeed;

		if (movement != 0)
			return MoveState;

		return null;
	}
}
