using Godot;

public partial class Resource : Area2D, IItem
{
    [Export]
    public GlobalTypes.ResourceType resourceType;
    public float ResourceValue { get; set; }
}
