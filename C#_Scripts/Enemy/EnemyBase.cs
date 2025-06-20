using Godot;
using System;
using System.Collections.Generic;

public partial class EnemyBase : CharacterBody2D
{
    [Export] public float Speed { get; set; } = 150f;
    [Export] public float Acceleration { get; set; } = 70f;
    [Export] public float AttackDamage { get; set; } = 5f;
    [Export] public float Health { get; set; } = 20f;

    [Export] public Sprite2D Animation { get; set; }

    [Export] public HitboxComponent HitboxComponent { get; set; }
    [Export] public Area2D DetectionAreaComponent { get; set; }

    private const float JUMP_VELOCITY = -400f;

    private Player player;
    private bool playerDetected = false;

    private double lastActionTime = 0.0;
    private float jumpCooldown = 1.5f;

    public Godot.Collections.Array<GlobalTypes.EnemyKillResult> KillResults = new();

    [Signal]
    public delegate void EnemyDiedEventHandler(EnemyBase enemy, Godot.Collections.Array<GlobalTypes.EnemyKillResult> enemyKillResult);

    [Export]
    public Godot.Collections.Array<NodePath> PatrolPoints { get; set; } = new();

    private int currentTargetIndex = 0;
    private Vector2 targetPosition;

    public override void _Ready()
    {
        SetupCharacter();

        if (PatrolPoints.Count > 0)
        {
            var node = GetNode<Node2D>(PatrolPoints[currentTargetIndex]);
            targetPosition = node.GlobalPosition;
        }

        RegisterEvents();
    }

    private void SetupCharacter()
    {
        player = GetTree().CurrentScene.GetNode<Player>("Player");
        if (player == null)
            GD.PrintErr("Player node not found");

        var healthComp = GetNode<HealthComponent>("HealthComponent");
        healthComp.Connect("EntityDied", new Callable(this, nameof(OnDied)));

        if (HitboxComponent != null)
            HitboxComponent.Set("health_component", healthComp);
    }

    private void OnDied(Godot.Collections.Array killResult)
    {
        EmitSignal(nameof(EnemyDied), this, killResult);
    }

    public override void _PhysicsProcess(double delta)
    {
        float dt = (float)delta;

        if (!IsOnFloor())
            Velocity += GetGravity() * dt;

        if (player != null && playerDetected)
        {
            // Update targetPosition from patrol points (optional)
            if (PatrolPoints.Count > 0)
            {
                var node = GetNode<Node2D>((NodePath)PatrolPoints[currentTargetIndex]);
                targetPosition = node.GlobalPosition;
            }

            // Jump logic
            if (!player.IsOnFloor() && (Time.GetTicksMsec() / 1000.0 - lastActionTime) >= jumpCooldown)
            {
                Velocity = new Vector2(Velocity.X, JUMP_VELOCITY);
                lastActionTime = Time.GetTicksMsec() / 1000.0;
            }

            float distance = player.GlobalPosition.DistanceTo(GlobalPosition);
            Vector2 direction = GlobalPosition.DirectionTo(player.GlobalPosition).Normalized();

            if (Animation != null)
                Animation.FlipH = direction.X < 0;

            Vector2 targetVelocity = direction * Speed;

            if (distance < 50)
                Velocity = Velocity.MoveToward(Vector2.Zero, Acceleration / 2 * dt);
            else
                Velocity = Velocity.MoveToward(targetVelocity, Acceleration * dt);
        }
        else
        {
            if (PatrolPoints.Count == 0)
                return;

            var node = GetNode<Node2D>((NodePath)PatrolPoints[currentTargetIndex]);
            targetPosition = node.GlobalPosition;

            Vector2 direction = GlobalPosition.DirectionTo(targetPosition).Normalized();

            if (Animation != null)
                Animation.FlipH = direction.X < 0;

            Vector2 targetVelocity = direction * Speed;

            Velocity = Velocity.MoveToward(targetVelocity, Acceleration * dt);

            if (GlobalPosition.DistanceTo(targetPosition) < 2)
            {
                currentTargetIndex++;
                if (currentTargetIndex >= PatrolPoints.Count)
                    currentTargetIndex = 0;
            }
        }

        MoveAndSlide();
    }

    private void RegisterEvents()
    {
        if (DetectionAreaComponent != null)
        {
            DetectionAreaComponent.Connect("body_entered", new Callable(this, nameof(OnDetectionEntered)));
            DetectionAreaComponent.Connect("body_exited", new Callable(this, nameof(OnDetectionExited)));
        }
    }

    private void OnDetectionEntered(Node body)
    {
        if (body.IsInGroup("Player"))
            playerDetected = true;
    }

    private void OnDetectionExited(Node body)
    {
        if (body.IsInGroup("Player"))
            playerDetected = false;
    }
}

