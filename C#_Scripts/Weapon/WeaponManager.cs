using Godot;

public partial class WeaponManager : ItemManager<Weapon>
{
	private Node2D _weaponHolder;

	public override void _Ready()
	{
		_weaponHolder = GetNode<Node2D>("WeaponHolder");
	}

	public override void Init(Player parent)
	{
		base.Init(parent);

		// Connect player signals
		_player.Attack += OnAttack;
		_player.Drop += OnDropCurrentWeapon;
		_player.Interact += OnPickUpWeapon;

		// Connect item signals
		_player.PickupArea.ItemNearbyEntered += OnWeaponNearbyEntered;
		_player.PickupArea.ItemNearbyExited += OnWeaponNearbyExited;
	}

	private void OnWeaponNearbyEntered(Area2D area)
	{
		if (area is Weapon weapon && !_itemsNearby.Contains(weapon))
		{
			_itemsNearby.Add(weapon);
		}
	}

	private void OnWeaponNearbyExited(Area2D area)
	{
		if (area is Weapon weapon && _itemsNearby.Contains(weapon))
		{
			_itemsNearby.Remove(weapon);
		}
	}

	private void OnAttack()
	{
		_currentItem?.Attack();
	}

	private Weapon PickNearestWeapon()
	{
		float shortestDistance = float.PositiveInfinity;
		Weapon closestWeapon = null;

		foreach (var weapon in _itemsNearby)
		{
			if (weapon == _currentItem)
				continue;

			float distance = weapon.GlobalPosition.DistanceTo(_player.GlobalPosition);
			if (distance < shortestDistance)
			{
				shortestDistance = distance;
				closestWeapon = weapon;
			}
		}

		return closestWeapon;
	}

	private void OnDropCurrentWeapon()
	{
		if (_currentItem == null)
			return;

		_weaponHolder.RemoveChild(_currentItem);
		GetTree().CurrentScene.AddChild(_currentItem); // Or _player.GetWorld() equivalent
		_currentItem.GlobalPosition = _player.GlobalPosition;
		_currentItem.GravityEnabled = true;
		_currentItem = null;
	}

	private void OnPickUpWeapon()
	{
		var weapon = PickNearestItem();
		if (weapon == null)
			return;

		OnDropCurrentWeapon();

		weapon.GetParent()?.RemoveChild(weapon);
		_weaponHolder.AddChild(weapon);

		weapon.Position = Vector2.Zero;
		weapon.GravityEnabled = false;

		_currentItem = weapon;
	}

	// Placeholder for animation contrrl
	public void SetAnimation(string animation)
	{
		// TODO: Implement animation logic
	}
}
