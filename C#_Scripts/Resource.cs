using Godot;

public partial class Resource : Item
{
    [Export]
    public GlobalTypes.ResourceType resourceType;
    public float ResourceValue { get; set; } = 2;
}
