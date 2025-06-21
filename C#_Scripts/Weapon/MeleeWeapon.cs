using Godot;

public partial class MeleeWeapon : Weapon
{
	public override void _Ready()
	{
		base._Ready();
		RegisterEvents();
	}

	private void RegisterEvents()
	{
		Connect("area_entered", new Callable(this, nameof(OnAttackEntered)));
	}

	private void OnAttackEntered(Node area)
	{
		if (area is HitboxComponent hitbox)
		{
			var attack = new Attack
			{
				AttackPosition = GlobalPosition,
				KnockbackForce = 20,
				AttackDamage = AttackDamage,
				WeaponType = GetType().Name
			};

			hitbox.Damage(attack);
		}
	}

	public override void Attack()
	{
		if (_animationPlayer != null)
		{
			_animationPlayer.Play("slash");
			//await ToSignal(Weapon._animationPlayer, "animation_finished");
		}
	}
}
