using Godot;

public partial class WeaponManager : ItemManager
{
	private Node2D _weaponHolder;
	private Weapon _currentWeapon;

	public Weapon GetCurrentWeapon { get { return _currentWeapon; } }
	public Node2D GetWeaponHolder { get { return _weaponHolder; } }

	public override void _Ready()
	{
		_weaponHolder = GetNode<Node2D>("WeaponHolder");
		if (_weaponHolder is null)
			GD.Print("WeaponHolder not found in WeaponManager of Player");
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
		_currentWeapon?.Attack();
	}

	private void OnDropCurrentWeapon()
	{
		if (_currentWeapon == null)
			return;

		_weaponHolder.RemoveChild(_currentWeapon);
		GetTree().CurrentScene.AddChild(_currentWeapon);
		_currentWeapon.GlobalPosition = player.GlobalPosition;
		_currentWeapon.GravityEnabled = true;
		_currentWeapon.IsItemEquipped = false;
		_currentWeapon = null;
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

		_currentWeapon = weapon;
		weapon.IsItemEquipped = true;
		player.PlayerAnimationState = weapon.WeaponAttackComponent is RangeAttackComponent
			? GlobalTypes.PlayerAnimationState.RANGEWEAPON 
			: GlobalTypes.PlayerAnimationState.MELEEWEAPON;
	}
}
