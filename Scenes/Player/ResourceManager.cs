using System.Collections.Generic;
using Godot;

public partial class ResourceManager : ItemManager
{
    // Tell ui that item is not pickable
	[Signal] public delegate void ResourceBackpackFullEventHandler();
    private const int BACKPACK_MAX_VALUE = 3;
    private Dictionary<GlobalTypes.ResourceType, float> playerResources = new Dictionary<GlobalTypes.ResourceType, float> {
        {GlobalTypes.ResourceType.ETHANOL, 0},
        {GlobalTypes.ResourceType.GUNPOWDER, 0},
        {GlobalTypes.ResourceType.PAPER, 0},
        {GlobalTypes.ResourceType.PLASTIC, 0},
        {GlobalTypes.ResourceType.TAPE, 0},
        {GlobalTypes.ResourceType.SCISSOR, 0},
    };

    public override void Init(Player parent, NearbyItemTracker nearbyItemTracker)
    {
        base.Init(parent, nearbyItemTracker);
        nearbyItemTracker.PlayerPickUp += PickUpResource;
    }

    private void PickUpResource(Area2D area)
    {
        if (area == null || area is not Resource resource)
            return;

        if (!SetPlayerResources(resource))
        {
            EmitSignal(SignalName.ResourceBackpackFull);
            return;
        }
        resource.QueueFree(); // TODO probably best that the ItemManager of the level does it?
    }

    /// <summary>
    /// Add resource to playerResources if there is space left
    /// </summary>
    /// <param name="resource">resource that should be added to backpack</param>
    /// <returns>Bool if resource was added to playerResources</returns>
    public bool SetPlayerResources(Resource resource)
    {
        var resourceType = resource.resourceType;
        var resourceValue = resource.ResourceValue;

        if (!playerResources.TryGetValue(resourceType, out float backpackValue))
        {
            GD.PushError($"ResourceType not known in playerResources Dictionary: {resourceType}");
            return false;
        }

        float newBackpackValue = backpackValue + resourceValue;
        // check if resource backpack full
        if (backpackValue >= BACKPACK_MAX_VALUE)
            return false;

        // set to max if resource would exceed max backpack value
        if (newBackpackValue > BACKPACK_MAX_VALUE)
            newBackpackValue = BACKPACK_MAX_VALUE;

        playerResources[resourceType] = newBackpackValue;
        return true;
    }
}
