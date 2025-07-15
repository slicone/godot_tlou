using Godot;

public partial class PlayerAnimationTree : Node2D
{
    [Export] public AnimationTree AnimationTree { get; set; }
    private bool idle = false;
    private bool run = false;
    private Player player;
    public void Init(Player player)
    {
        this.player = player;
    }

    public void SetIdleAnimation(GlobalTypes.PlayerAnimationState playerAnimationState)
    {
        idle = true;
        run = false;
        MakeSpriteVisibleOnCondition(playerAnimationState);
    }
    public void SetRunAnimation(GlobalTypes.PlayerAnimationState playerAnimationState)
    {
        idle = false;
        run = true;
        MakeSpriteVisibleOnCondition(playerAnimationState);
    }

    public void MakeSpriteVisibleOnCondition(GlobalTypes.PlayerAnimationState playerAnimationState)
    {
        if (player.currentSprite != null)
            player.currentSprite.Visible = false;

        switch (playerAnimationState)
        {
            case GlobalTypes.PlayerAnimationState.NOGUN:
            {
                var spriteNameNoGun = "Player";
                var spriteNoGun = player.GetNode<Sprite2D>(spriteNameNoGun);

                if (spriteNoGun is null)
                    GD.PrintErr($"Sprite {spriteNameNoGun} is null");

                spriteNoGun.Visible = true;
                player.currentSprite = spriteNoGun;
                break; 
            }
            case GlobalTypes.PlayerAnimationState.RANGEWEAPON: {
                var spriteNameRange = "PlayerWithGun";
                var spriteRange = player.GetNode<Node2D>(spriteNameRange);
                if (spriteNameRange is null)
                    GD.PrintErr($"Sprite {spriteNameRange} is null");

                spriteRange.Visible = true;
                player.currentSprite = spriteRange;
                break;
            }
        }
    }

    private float lastFacingDirectionNotZero = 0;

    public override void _PhysicsProcess(double delta)
    {
        var currentFacingDirection = player.Velocity.Normalized().X;

        if (currentFacingDirection != 0)
        {
            lastFacingDirectionNotZero = currentFacingDirection;
        }

        // Blend pos 
        AnimationTree.Set("parameters/Idle/blend_position", lastFacingDirectionNotZero);
        AnimationTree.Set("parameters/Idle_Range/blend_position", lastFacingDirectionNotZero);
        AnimationTree.Set("parameters/Run_Range/blend_position", lastFacingDirectionNotZero); 
        AnimationTree.Set("parameters/Run/blend_position", lastFacingDirectionNotZero);
    
        // State change
        AnimationTree.Set("parameters/conditions/idle", idle);
        AnimationTree.Set("parameters/conditions/run", run);

    }
}