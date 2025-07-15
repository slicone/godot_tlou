using Godot;
using System;

public partial class RangeAttackComponent : AttackComponent
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
		if (RayCast is null || FireTimer is null || Weapon is null || Weapon.AnimationPlayer is null)
		{
			GD.PushError("Missing dependencies in RangeAttackComponent");
			return;
		}
		_currentMagazineRounds = MaxMagazineRounds;
		SetupTimer();
		Weapon.AnimationPlayer.AnimationFinished += OnAnimationFinished;
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
			Weapon.AnimationPlayer?.Play("shoot");
			Shoot();
		}
	}

	private void SetupTimer()
	{
		FireTimer.WaitTime = FireRate;
		FireTimer.OneShot = true;
		FireTimer.Timeout += OnFireTimerTimeout;
	}

	private void Shoot()
	{
		_canFire = false;

		if (_currentMagazineRounds <= 0)
			return;

		_currentMagazineRounds--;
		Weapon.AnimationPlayer.Play("Shoot");
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
		if (_currentMagazineRounds < MaxMagazineRounds && !Weapon.AnimationPlayer.IsPlaying())
		{
			_canFire = false;
			Weapon.AnimationPlayer.Play("Reload");
		}
	}

	private void OnFireTimerTimeout()
	{
		_canFire = true;
	}

	private void OnAnimationFinished(StringName animName)
	{
		switch (animName)
		{
			case "Reload":
				ReloadAnimationFinished();
				break;
		}
	}

	private void ReloadAnimationFinished()
	{
		_currentMagazineRounds = MaxMagazineRounds;
		_canFire = true;
	}
}
