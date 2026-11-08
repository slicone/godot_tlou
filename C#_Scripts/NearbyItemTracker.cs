using Godot;
using System.Collections.Generic;

public partial class NearbyItemTracker : Node
{
    [Signal] public delegate void PlayerPickUpEventHandler(Area2D area);
    // work with Area2D because IItem can't be send in signal
    protected List<Area2D> _itemsNearby = [];
    protected Player _player;
    public void Init(Player parent)
    {
        if(parent == null)
            GD.PushError($"Missing dependencies in {GetType().Name}");   

        _player = parent;
        // player signals
        _player.Interact += PickNearestItem;
		// Connect item signals
        _player.PickupArea.ItemNearbyEntered += ItemNearbyEntered;
		_player.PickupArea.ItemNearbyExited += ItemNearbyExited;
    }

	private void ItemNearbyEntered(Area2D area)
    {
        if (area is IItem && !_itemsNearby.Contains(area))
        {
            _itemsNearby.Add(area);
        }
    }

	private void ItemNearbyExited(Area2D area)
	{
		if (area is IItem && _itemsNearby.Contains(area))
        {
            _itemsNearby.Remove(area);
        }
	}

    private void PickNearestItem()
    {
        float shortestDistance = float.PositiveInfinity;
        Area2D closestItem = null;

        foreach (var item in _itemsNearby)
        {
            // skip if player has item equipped
            if (item is IItem item_cast && item_cast.IsItemEquipped)
                continue;

            float distance = item.GlobalPosition.DistanceTo(_player.GlobalPosition);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestItem = item;
            }
        }

        EmitSignal(SignalName.PlayerPickUp, closestItem);
    }
}
