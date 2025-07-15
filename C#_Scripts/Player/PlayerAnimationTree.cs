using Godot;

public partial class PlayerAnimationTree : Node2D
{
    [Export] public AnimationTree AnimationTree { get; set; }
    private Player player;
    public void Init(Player player)
    {
        this.player = player;
    }

    public void SetIdleAnimation(bool showIdleAnimation)
    {
        AnimationTree.Set("parameters/conditions/idle", showIdleAnimation);
        if (showIdleAnimation) // only show if state is entered, because next state will call this method anyways
            MakePlayerAnimationStateSpriteVisibleOnState();
    }
    public void SetRunAnimation(bool showRunAnimation)
    {
        AnimationTree.Set("parameters/conditions/run", showRunAnimation);
        if (showRunAnimation)
            MakePlayerAnimationStateSpriteVisibleOnState();
    }

    public void SetBackpackAnimation(bool showBackpackAnimation)
    {
        AnimationTree.Set("parameters/conditions/backpack", showBackpackAnimation);
        if (showBackpackAnimation)
            MakePlayerAnimationStateSpriteVisibleOnState();
    }

    /// <summary>
    /// There is a different sprite for each player animation state.
    /// Sprite themselves have all the needed animation for each state like run, idle...
    /// This is necessary because if there is a sprite for each of these states, there are weird animation bugs
    /// with the AnimationTree. As a result the animations of these states are handeled in the AnimationTree.
    /// </summary>
    private void MakePlayerAnimationStateSpriteVisibleOnState()
    {
        if (player.currentSprite != null)
            player.currentSprite.Visible = false;

        switch (player.PlayerAnimationState)
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
            case GlobalTypes.PlayerAnimationState.RANGEWEAPON:
                {
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

    private void CheckIfWeaponMustBeFlipped()
    {
        var playerCurrentWeapon = player.WeaponManager.GetCurrentWeapon;
        var playerWeaponHolder = player.WeaponManager.GetWeaponHolder;

        if (player.PlayerAnimationState != GlobalTypes.PlayerAnimationState.NOGUN && playerCurrentWeapon is not null && playerWeaponHolder is not null)
        {
            playerCurrentWeapon.FlipSprite(lastFacingDirectionNotZero < 0);
            if (playerWeaponHolder.Position.X > 0 && lastFacingDirectionNotZero < 0 || playerWeaponHolder.Position.X < 0 && lastFacingDirectionNotZero > 0)
            {
                playerWeaponHolder.Position = new Vector2(playerWeaponHolder.Position.X * -1, playerWeaponHolder.Position.Y);
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

        CheckIfWeaponMustBeFlipped();
    }
}