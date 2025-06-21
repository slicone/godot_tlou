using Godot;

public partial class Attack : Node
{
    public float AttackDamage { get; set; }
    public float KnockbackForce { get; set; }
    public Vector2 AttackPosition { get; set; }
    public string WeaponType { get; set; }
}

