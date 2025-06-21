using Godot;
using System;

public partial class MoveState : State
{
	[Export] public State IdleState { get; set; }
	[Export] public State FallState { get; set; }
	[Export] public State JumpState { get; set; }

	private float timeSinceInput = 0f;

	public override State ProcessInput(InputEvent @event)
	{
		CheckNonStateInput();

		if (Input.IsActionJustPressed("jump") && Parent.IsOnFloor())
			return JumpState;

		return null;
	}

	public override State ProcessPhysics(double delta)
	{
		float movement = Input.GetAxis("move-left", "move-right") * Parent.MoveSpeed;

		if (!Parent.IsOnFloor())
			return FallState;

		if (movement != 0)
		{
			if (Animation != null)
				Animation.FlipH = movement < 0;

			// Optional: flip weapon holder and current weapon sprite if needed
			// if (Parent.WeaponHolder.Position.x != 0 && Parent.CurrentWeapon != null)
			// {
			//     Parent.WeaponHolder.Position = new Vector2(-Parent.WeaponHolder.Position.x, Parent.WeaponHolder.Position.y);
			//     Parent.CurrentWeapon.FlipSprite();
			// }

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
