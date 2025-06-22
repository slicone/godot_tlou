using Godot;

public abstract partial class AttackComponent : Area2D, IAttackComponent
{
    [Export] public Weapon Weapon { get; set; }
    [Export] public float AttackDamage { get; set; } = 20.0f;

    public abstract void Attack();
}

