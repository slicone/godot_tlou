using Godot;

public partial class Weapon : Item
{

	[Export]
	public AttackComponent WeaponAttackComponent { get; set; }

	[Export]
	public bool GravityEnabled { get; set; } = true;

	private Vector2 _velocity = Vector2.Zero;

	[Export]
	public AnimationPlayer AnimationPlayer { get; set; }
	
	public override void _PhysicsProcess(double delta)
	{
		if (!GravityEnabled)
			return;

		if (ContainsStaticBody(GetOverlappingBodies()))
		{
			_velocity = Vector2.Zero;
			return;
		}

		_velocity.Y += Gravity * (float)delta;
		Position += _velocity * (float)delta;
	}

	private bool ContainsStaticBody(Godot.Collections.Array<Node2D> bodies)
	{
		foreach (var body in bodies)
		{
			if (body is StaticBody2D || body is TileMapLayer)
				return true;
		}
		return false;
	}

	// TODO obsolete?
	public void FlipSprite()
	{
		foreach (Node child in GetChildren())
		{
			if (child is Sprite2D sprite)
			{
				sprite.FlipH = !sprite.FlipH;
			}
		}
	}

	public void Attack()
	{
		WeaponAttackComponent?.Attack();
	}
}
