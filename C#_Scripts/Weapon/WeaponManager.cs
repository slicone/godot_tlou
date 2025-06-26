using Godot;

public partial class WeaponManager : ItemManager
{
	private Node2D _weaponHolder;
	private Weapon currentWeapon;

	public override void _Ready()
	{
		_weaponHolder = GetNode<Node2D>("WeaponHolder");
	}

	public override void Init(Player parent, NearbyItemTracker nearbyItemTracker)
	{
		base.Init(parent, nearbyItemTracker);

		// Connect player signals
		player.Attack += OnAttack;
		player.Drop += OnDropCurrentWeapon;

		nearbyItemTracker.PlayerPickUp += PickUpWeapon;
	}

	private void OnAttack()
	{
		currentWeapon?.Attack();
	}

	private void OnDropCurrentWeapon()
	{
		if (currentWeapon == null)
			return;

		_weaponHolder.RemoveChild(currentWeapon);
		GetTree().CurrentScene.AddChild(currentWeapon); // Or _player.GetWorld() equivalent
		currentWeapon.GlobalPosition = player.GlobalPosition;
		currentWeapon.GravityEnabled = true;
		currentWeapon.IsItemEquipped = false;
		currentWeapon = null;
		player.PlayerAnimationState = GlobalTypes.PlayerAnimationState.NOGUN;
	}

	private void PickUpWeapon(Area2D area)
	{
		if (area == null || area is not Weapon weapon)
			return;

		OnDropCurrentWeapon();

		weapon.GetParent()?.RemoveChild(weapon);
		_weaponHolder.AddChild(weapon);

		weapon.Position = Vector2.Zero;
		weapon.GravityEnabled = false;

		currentWeapon = weapon;
		weapon.IsItemEquipped = true;
		player.PlayerAnimationState = weapon.WeaponAttackComponent is RangeAttackComponent
			? GlobalTypes.PlayerAnimationState.RANGEWEAPON 
			: GlobalTypes.PlayerAnimationState.MELEEWEAPON;
	}

	// Placeholder for animation control
	public void SetAnimation(Weapon weapon)
	{
		// TODO maybe if Weapon has multiple animations? Like player gets upgrade for example an now different animation?	
	}
}
