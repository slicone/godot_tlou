using Godot;
using System;
using System.Collections.Generic;

public partial class ItemManager<T> : Node2D where T : Area2D, IItem
{
    protected Player _player;
    protected List<T> _itemsNearby = new();
    protected T _currentItem;

    public virtual void Init(Player parent)
    {
        _player = parent;
    }

    protected T PickNearestItem()
    {
        float shortestDistance = float.PositiveInfinity;
        T closestWeapon = null;

        foreach (var item in _itemsNearby)
        {
            if (item == _currentItem)
                continue;

            float distance = item.GlobalPosition.DistanceTo(_player.GlobalPosition);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closestWeapon = item;
            }
        }

        return closestWeapon;
    }
}
