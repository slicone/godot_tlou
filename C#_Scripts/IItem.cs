using Godot;
using System;

interface IItem
{
    /// <summary>
    /// Execute logic if item has entered an area
    /// </summary>
    public void ItemNearby();
}
