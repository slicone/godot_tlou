using Godot;

public partial class FallState : State
{
	[Export] public State IdleState { get; set; }
	[Export] public State MoveState { get; set; }

	public override State ProcessInput(InputEvent @event)
	{
		CheckNonStateInput();
		return null;
	}

	public override State ProcessPhysics(double delta)
	{
		if (Parent == null)
			return null;

		// Apply gravity
		Parent.Velocity += Parent.GetGravity() * (float)delta;

		// Horizontal movement
		float input = Input.GetAxis("move-left", "move-right");
		float movement = input * Parent.MoveSpeed;

		Parent.Velocity = new Vector2(movement, Parent.Velocity.Y);
		Parent.MoveAndSlide();

		// State transitions
		if (Parent.IsOnFloor())
		{
			if (movement != 0)
				return MoveState;
			return IdleState;
		}

		return null;
	}
}
