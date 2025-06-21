using Godot;
using System;
using System.Collections.Generic;

public partial class WeaponManager : Node2D
{
	private Player _player;
	private Weapon _currentWeapon;

	private List<Weapon> _weaponsNearby = new();

	[Export]
	public PickupArea PickupArea { get; set; }

	private Node2D _weaponHolder;

	public override void _Ready()
	{
		_weaponHolder = GetNode<Node2D>("WeaponHolder");
	}

	public void Init(Player parent)
	{
		_player = parent;

		// Connect player signals
		_player.Attack += OnAttack;
		_player.Drop += OnDropCurrentWeapon;
		_player.Interact += OnPickUpWeapon;

		// Connect item signals
		PickupArea.ItemNearbyEntered += OnWeaponNearbyEntered;
		PickupArea.ItemNearbyExited += OnWeaponNearbyExited;
	}

	private void OnWeaponNearbyEntered(Area2D area)
	{
		if (area is Weapon weapon && !_weaponsNearby.Contains(weapon))
		{
			_weaponsNearby.Add(weapon);
		}
	}

	private void OnWeaponNearbyExited(Area2D area)
	{
		if (area is Weapon weapon && _weaponsNearby.Contains(weapon))
		{
			_weaponsNearby.Remove(weapon);
		}
	}

	private void OnAttack()
	{
		_currentWeapon?.Attack();
	}

	private Weapon PickNearestWeapon()
	{
		float shortestDistance = float.PositiveInfinity;
		Weapon closestWeapon = null;

		foreach (var weapon in _weaponsNearby)
		{
			if (weapon == _currentWeapon)
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
		if (_currentWeapon == null)
			return;

		_weaponHolder.RemoveChild(_currentWeapon);
		GetTree().CurrentScene.AddChild(_currentWeapon); // Or _player.GetWorld() equivalent
		_currentWeapon.GlobalPosition = _player.GlobalPosition;
		_currentWeapon.GravityEnabled = true;
		_currentWeapon = null;
	}

	private void OnPickUpWeapon()
	{
		var weapon = PickNearestWeapon();
		if (weapon == null)
			return;

		OnDropCurrentWeapon();

		weapon.GetParent()?.RemoveChild(weapon);
		_weaponHolder.AddChild(weapon);

		weapon.Position = Vector2.Zero;
		weapon.GravityEnabled = false;

		_currentWeapon = weapon;
	}

	// Placeholder for animation control
	public void SetAnimation(string animation)
	{
		// TODO: Implement animation logic
	}
}
