using Godot;
using System;

public partial class Player : CharacterBody2D
{
	//public Weapon CurrentWeapon { get; set; }
	[Export] public AnimationPlayer AnimationPlayer { get; set; }
	[Export] public StateMachine StateMachine { get; set; }
	[Export] public WeaponManager WeaponManager { get; set; }
	[Export] public HitboxComponent HitboxComponent { get; set; }
	[Export] public HealthComponent HealthComponent { get; set; }
	[Export] public PickupArea PickupArea { get; set; }

	[Signal] public delegate void PlayerDiedEventHandler();
	[Signal] public delegate void AttackEventHandler();
	[Signal] public delegate void InteractEventHandler();
	[Signal] public delegate void DropEventHandler();

	public override void _Ready()
	{
		if (HealthComponent != null)
			HealthComponent.Connect("EntityDied", new Callable(this, nameof(OnPlayerDied)));

		if (HitboxComponent != null && HealthComponent != null)
			HitboxComponent.HealthComponent = HealthComponent;

		StateMachine.Init(this);
		WeaponManager?.Init(this);
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

	// Diese können z.B. von State-Maschinen aufgerufen werden
	public void TriggerAttack() => EmitSignal(SignalName.Attack);
	public void TriggerInteract() => EmitSignal(SignalName.Interact);
	public void TriggerDrop() => EmitSignal(SignalName.Drop);
}
