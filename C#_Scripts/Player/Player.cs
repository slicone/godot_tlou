using Godot;
using System;

public partial class Player : CharacterBody2D
{
	//[Export] public AnimationPlayer AnimationPlayer { get; set; }
	[Export] public PlayerAnimationTree PlayerAnimationTree { get; set; }
	[Export] public StateMachine StateMachine { get; set; }
	[Export] public HitboxComponent HitboxComponent { get; set; }
	[Export] public HealthComponent HealthComponent { get; set; }
	[Export] public NearbyItemTracker NearbyItemTracker { get; set; }
	[Export] public PickupArea PickupArea { get; set; }
	[Export] public float MoveSpeed { get; set; } = 299f;
	[Export] public float StopSpeed { get; set; } = 999f;
	[Export] public float JumpVelocity { get; set; } = -302.0f;

	[Signal] public delegate void PlayerDiedEventHandler();
	[Signal] public delegate void AttackEventHandler();
	[Signal] public delegate void InteractEventHandler();
	[Signal] public delegate void DropEventHandler();
	private GlobalTypes.PlayerAnimationState _playerAnimationState = GlobalTypes.PlayerAnimationState.NOGUN;

	public GlobalTypes.PlayerAnimationState PlayerAnimationState
	{
		get => _playerAnimationState;
		// always notify state_machine that state of player has to be refreshed
		set
		{
			_playerAnimationState = value;
			StateMachine?.PlayerAnimationStateChanged();
		}
	}

	// Node because sometimes Sprites can be collected in a Node2D
	// Is used to make current sprite not visible for next state
	public Node2D currentSprite;

	public override void _Ready()
	{
		if (HealthComponent != null)
			HealthComponent.Connect("EntityDied", new Callable(this, nameof(OnPlayerDied)));

		if (HitboxComponent != null && HealthComponent != null)
			HitboxComponent.HealthComponent = HealthComponent;

		PlayerAnimationTree.Init(this);
		StateMachine.Init(this);
		InitItemManagers();
	}

	private void InitItemManagers()
	{
		NearbyItemTracker.Init(this);
		foreach (var child in GetChildren())
		{
			if (child is IItemManager itemManager)
			{
				itemManager.Init(this, NearbyItemTracker);
			}
		}
	}

	private void OnPlayerDied(Godot.Collections.Array<GlobalTypes.EnemyKillResult> killResult)
	{
		EmitSignal(SignalName.PlayerDied);
	}

	public override void _UnhandledInput(InputEvent inputEvent)
	{
		StateMachine.ProcessInput(inputEvent);
	}

	public override void _Process(double delta)
	{
		StateMachine.ProcessFrame(delta);
	}

	public override void _PhysicsProcess(double delta)
	{
		StateMachine.ProcessPhysics(delta);
	}

	public void TriggerAttack() => EmitSignal(SignalName.Attack);
	public void TriggerInteract() => EmitSignal(SignalName.Interact);
	public void TriggerDrop() => EmitSignal(SignalName.Drop);
}
