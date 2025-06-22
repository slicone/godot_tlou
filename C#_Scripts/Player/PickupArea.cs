using Godot;
using System;

public partial class PickupArea : Area2D
{
	[Signal]
	public delegate void ItemNearbyEnteredEventHandler(Area2D area);

	[Signal]
	public delegate void ItemNearbyExitedEventHandler(Area2D area);

	public override void _Ready()
	{
		RegisterEvents();
	}

	private void RegisterEvents()
	{
		this.AreaEntered += OnEntered;
		this.AreaExited += OnExited;
	}

	private void OnEntered(Area2D area)
	{
		if (area is IItem)
			EmitSignal(SignalName.ItemNearbyEntered, area);
	}

	private void OnExited(Area2D area)
	{
		if(area is IItem)
			EmitSignal(SignalName.ItemNearbyExited, area);
	}
}
