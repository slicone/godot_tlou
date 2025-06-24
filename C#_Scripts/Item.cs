using Godot;
using System;

public partial class Item : Area2D, IItem
{
    public bool IsItemEquipped { get; set; } = false;
}
