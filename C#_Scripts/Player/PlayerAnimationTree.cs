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

        //AnimationTree.Set("parameters/conditions/idle", true);
        //AnimationTree.Set("parameters/conditions/run", false);
        idle = true;
        run = false;
        //  MakeIdleSpriteVisibleOnCondition(playerAnimationState);
    }

    public void MakeIdleSpriteVisibleOnCondition(GlobalTypes.PlayerAnimationState playerAnimationState)
    {
        //if (player.currentSprite != null)
        //    player.currentSprite.Visible = false;

        switch (playerAnimationState)
        {
            case GlobalTypes.PlayerAnimationState.NOGUN:
            {
                var spriteWithOut = player.GetNode<Sprite2D>("Idle");
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
        //AnimationTree.Set("parameters/conditions/run", true);
        //AnimationTree.Set("parameters/conditions/idle", false);
        idle = false;
        run = true;
        MakeRunSpriteVisibleOnCondition(playerAnimationState);
    }
    public void MakeRunSpriteVisibleOnCondition(GlobalTypes.PlayerAnimationState playerAnimationState)
    {
        //if (player.currentSprite != null)
        //    player.currentSprite.Visible = false;

        switch (playerAnimationState)
        {
            case GlobalTypes.PlayerAnimationState.NOGUN:
                {
                    var spriteWithOut = player.GetNode<Sprite2D>("Run");
                    spriteWithOut.Visible = true;
                    player.currentSprite = spriteWithOut;
                    break;
                }
            case GlobalTypes.PlayerAnimationState.RANGEWEAPON:
                {
                    var spriteWithOut = player.GetNode<Node2D>("RangeWeaponRun");
                    spriteWithOut.Visible = true;
                    player.currentSprite = spriteWithOut;
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

        if ((bool)AnimationTree.Get("parameters/conditions/idle"))
        {
        }

        if ((bool)AnimationTree.Get("parameters/conditions/run"))
        {
        }
     
            AnimationTree.Set("parameters/Idle/blend_position", lastFacingDirectionNotZero);
            AnimationTree.Set("parameters/Idle_Range/blend_position", lastFacingDirectionNotZero);
            AnimationTree.Set("parameters/Run/blend_position", lastFacingDirectionNotZero);
            AnimationTree.Set("parameters/Run_Range/blend_position", lastFacingDirectionNotZero); 
        AnimationTree.Set("parameters/conditions/idle", idle);
        AnimationTree.Set("parameters/conditions/run", run);

        // Immer den letzten gültigen Wert verwenden
    }
}