using Godot;

public partial class MeleeAttackComponent : AttackComponent
{
	public override void _Ready()
	{
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
				AttackComponent = this
			};

			hitbox.Damage(attack);
		}
	}

	public override void Attack()
	{
		if (Weapon?._animationPlayer != null)
		{
			Weapon._animationPlayer.Play("slash");
			//await ToSignal(Weapon._animationPlayer, "animation_finished");
		}
	}
}
