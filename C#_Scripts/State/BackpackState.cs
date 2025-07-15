using Godot;

public partial class BackpackState : State
{
	[Export] public State IdleState { get; set; }

    public override void Enter()
    {
        Parent.PlayerAnimationTree.SetBackpackAnimation(true);
    }

    public override void Exit()
    {
        Parent.PlayerAnimationTree.SetBackpackAnimation(false);
    }
    
	public override State ProcessInput(InputEvent @event)
    {
        if (Input.IsActionJustPressed("backpack"))
            return IdleState;

        return null;
    }

	public override State ProcessPhysics(double delta)
	{
        return null;
	}
}
