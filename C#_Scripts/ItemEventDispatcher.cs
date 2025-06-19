using Godot;
using System;

/// <summary>
/// Sends nearby item signal dependend on item type
/// </summary>
public partial class ItemEventDispatcher : Node
{
    [Export]
    public PickupArea PickupAreaReference { get; set; }
    [Signal]
    public delegate void WeaponEnteredEventHandler(Area2D weapon); // TODO make Weapon class in c#

     // Singleton-Instance
    public static ItemEventDispatcher Instance { get; private set; }

    public override void _EnterTree()
    {
        Instance = this;
    }

    public void EmitWeaponPickedUp(Area2D weapon) // TODO make Weapon
    {
        EmitSignal(SignalName.WeaponEntered, weapon);
    }

    public override void _Ready()
    {
        this.PickupAreaReference.ItemNearbyEntered += OnItemEntered;
    }

    public void OnItemEntered(Area2D item)
    {
        //if (item is Weapon weapon)
        //EmitSignal(SignalName.WeaponEntered, weapon);
        // Weitere Typen hier
        EmitSignal(SignalName.WeaponEntered, item);
    }
}
