using Godot;

public partial class PlayerAnimationTree : Node2D
{
    [Export] public AnimationTree AnimationTree { get; set; }
    private Player player;
    public void Init(Player player)
    {
        this.player = player;
    }

    public void SetIdleAnimation(GlobalTypes.PlayerAnimationState playerAnimationState)
    {
        AnimationTree.Set("parameters/conditions/idle", true);
        AnimationTree.Set("parameters/conditions/run", false);
        MakeIdleSpriteVisibleOnCondition(playerAnimationState);
    }

    public void MakeIdleSpriteVisibleOnCondition(GlobalTypes.PlayerAnimationState playerAnimationState)
    {
        if (player.currentSprite != null)
            player.currentSprite.Visible = false;

        switch (playerAnimationState)
        {
            case GlobalTypes.PlayerAnimationState.NOGUN:
            {
                var spriteWithOut = player.GetNode<Node2D>("Idle");
                spriteWithOut.Visible = true;
                player.currentSprite = spriteWithOut;
                break;    
            }
            case GlobalTypes.PlayerAnimationState.RANGEWEAPON: {
                var sprite = player.GetNode<Node2D>("RangeWeaponIdle");
                sprite.Visible = true;
                player.currentSprite = sprite;
                break;
            }
        }
    }

    public void SetRunAnimation(GlobalTypes.PlayerAnimationState playerAnimationState)
    {
        AnimationTree.Set("parameters/conditions/run", true);
        AnimationTree.Set("parameters/conditions/idle", false);
        MakeRunSpriteVisibleOnCondition(playerAnimationState);
    }
    public void MakeRunSpriteVisibleOnCondition(GlobalTypes.PlayerAnimationState playerAnimationState)
    {
        if (player.currentSprite != null)
            player.currentSprite.Visible = false;

        switch (playerAnimationState)
        {
            case GlobalTypes.PlayerAnimationState.NOGUN:
                {
                    var spriteWithOut = player.GetNode<Node2D>("Run");
                    spriteWithOut.Visible = true;
                    player.currentSprite = spriteWithOut;
                    break;
                }
            case GlobalTypes.PlayerAnimationState.RANGEWEAPON:
                {
                    //var sprite = player.GetNode<Node2D>("RangeWeaponIdle");
                    //sprite.Visible = true;
                    //player.currentSprite = sprite;
                    break;
                }
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        AnimationTree.Set("parameters/Run/blend_position", player.Velocity.Normalized());
        AnimationTree.Set("parameters/Idle/blend_position", player.Velocity.Normalized());
    }
}
