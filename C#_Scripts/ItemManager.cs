using Godot;

public partial class ItemManager : Node, IItemManager
{
    protected Player player;
    protected NearbyItemTracker nearbyItemTracker;

    public virtual void Init(Player parent, NearbyItemTracker nearbyItemTracker)
    {
        if (player == null || nearbyItemTracker == null)
            GD.PushError($"Missing dependencies in {GetType().Name}");   
        player = parent;
        this.nearbyItemTracker = nearbyItemTracker;
    }
}
