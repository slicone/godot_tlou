using Godot;
using System;

public partial class RangeWeapon : Weapon
{
	[Export]
	public float FireRate { get; set; } = 0.5f;
	[Export]
	public float ReloadTime { get; set; } = 2.0f;
	[Export]
	public int MaxMagazineRounds { get; set; } = 6;

	private int _currentMagazineRounds;
	private bool _canFire = true;


	public RayCast2D RayCast { get; set; }
	public Timer FireTimer { get; set; }
	public Timer ReloadTimer { get; set; }

	public override void _Ready()
	{
		RayCast = GetNodeOrNull<RayCast2D>("RayCast2D");
		FireTimer = GetNodeOrNull<Timer>("FireTimer");
		ReloadTimer = GetNodeOrNull<Timer>("ReloadTimer");
		if (RayCast is null || FireTimer is null || ReloadTimer is null)
		{
			GD.PushError("Missing dependencies in RangeAttackComponent");
			return;
		}
		_currentMagazineRounds = MaxMagazineRounds;
		SetupTimers();
		RayCast.CollideWithAreas = true;
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("reload"))
			Reload();
	}

	public override void Attack()
	{
		if (_canFire)
		{
			_animationPlayer?.Play("shoot");
			Shoot();
		}
	}

	private void SetupTimers()
	{
		FireTimer.WaitTime = FireRate;
		ReloadTimer.WaitTime = ReloadTime;
		FireTimer.OneShot = true;
		ReloadTimer.OneShot = true;

		FireTimer.Timeout += OnFireTimerTimeout;
		ReloadTimer.Timeout += OnReloadTimerTimeout;
	}

	private void Shoot()
	{
		_canFire = false;

		if (_currentMagazineRounds <= 0)
			return;

		_currentMagazineRounds--;
		FireTimer.Start();

		Vector2 mousePos = GetGlobalMousePosition();
		Vector2 dir = (mousePos - GlobalPosition).Normalized();

		RayCast.TargetPosition = dir * 1000;
		RayCast.ForceRaycastUpdate();

		if (RayCast.IsColliding())
		{
			var hit = RayCast.GetCollider();
			if (hit is HitboxComponent hitbox)
			{
				var attack = new Attack
				{
					AttackDamage = AttackDamage,
					AttackPosition = GlobalPosition,
					KnockbackForce = 5000,
					WeaponType = GetType().Name
				};
				hitbox.Damage(attack);
			}
		}
	}

	private void Reload()
	{
		_canFire = false;
		ReloadTimer.Start();
	}

	private void OnFireTimerTimeout()
	{
		_canFire = true;
	}

	private void OnReloadTimerTimeout()
	{
		_currentMagazineRounds = MaxMagazineRounds;
		_canFire = true;
	}
}
