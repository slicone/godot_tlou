using System.Collections.Generic;
using Godot;

public partial class ResourceManager : ItemManager<Resource>
{
    private Dictionary<GlobalTypes.ResourceType, float> _playerResources = new Dictionary<GlobalTypes.ResourceType, float> {
        {GlobalTypes.ResourceType.ETHANOL, 0},
        {GlobalTypes.ResourceType.GUNPOWDER, 0},
        {GlobalTypes.ResourceType.PAPER, 0},
        {GlobalTypes.ResourceType.PLASTIC, 0},
        {GlobalTypes.ResourceType.TAPE, 0},
        {GlobalTypes.ResourceType.SCISSOR, 0},
    };


    public override void Init(Player parent)
    {
        base.Init(parent);
        // Connect player signals
        _player.PickupArea.ItemNearbyEntered += ResourceNearbyEntered;
        _player.PickupArea.ItemNearbyExited += ResourceNearbyExited;
    }

    public void ResourceNearbyEntered(Area2D area)
    {
        if (area is Resource resource && !_itemsNearby.Contains(resource))
		{
			_itemsNearby.Add(resource);
		}     
    }

    public void ResourceNearbyExited(Area2D area)
    {
        if (area is Resource resource && _itemsNearby.Contains(resource))
		{
			_itemsNearby.Remove(resource);
		} 
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
        if (!_playerResources.TryGetValue(resourceType, out float backpackValue))
        {
            GD.PushError($"ResourceType not known in playerResources Dictionary: {resourceType}");
            return false;
        }
        float newBackpackValue = backpackValue + resourceValue;
        // check if resource backpack full
        if (backpackValue >= 3 || newBackpackValue > 3)
            return false;

        _playerResources[resourceType] = newBackpackValue;
        return true;
    }
}
