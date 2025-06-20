using Godot;
using System;


public partial class HealthComponent : Node2D
{
    [Export]
    public float MAX_HEALTH { get; set; } = 100.0f;

    [Signal]
    public delegate void HealthChangedEventHandler();

    [Signal]
    public delegate void EntityDiedEventHandler(Godot.Collections.Array killResult);

    [Signal]
    public delegate void EntityTookDamageEventHandler(int damage);

    [Export]
    public float Health { get; set; }

    public override void _Ready()
    {
        Health = MAX_HEALTH;
    }

    public void Damage(Attack attack)
    {
        if (!(GetParent() is CharacterBody2D entity))
        {
            GD.PushError("HealthComponent needs a CharacterBody2D as parent");
            return;
        }

        ForceEntityKnockback(attack, entity);
        CalculateEntityHealth(attack);
        CheckIfEntityDies(entity, attack.AttackComponent);
    }

    private void CalculateEntityHealth(Attack attack)
    {
        Health -= attack.AttackDamage;
        EmitSignal(SignalName.HealthChanged);
        EmitSignal(SignalName.EntityTookDamage, (int) attack.AttackDamage);
    }

    private void CheckIfEntityDies(CharacterBody2D entity, IAttackComponent attackComponent)
    {
        if (Health <= 0)
        {
            var killResult = GetEntityKillResult(entity, attackComponent);
            EmitSignal(SignalName.EntityDied, killResult);
            entity.QueueFree();
        }
    }

    
    private Godot.Collections.Array<GlobalTypes.EnemyKillResult> GetEntityKillResult(CharacterBody2D entity, IAttackComponent attackComponent)
    {
        var killResult = new Godot.Collections.Array<GlobalTypes.EnemyKillResult>();
        if (entity is EnemyBase enemy)
        {
            killResult.Add(DetermineWeaponType(attackComponent));
            var entityKillResults = enemy.KillResults;
            foreach (var result in entityKillResults)
            {
                killResult.Add(result);
            }
        }
        return killResult;
    }

    private void ForceEntityKnockback(Attack attack, CharacterBody2D entity)
    {
        // Richtung vom Entity zur Attack Position (Kraftvektor)
        entity.Velocity = entity.GlobalPosition.DirectionTo(attack.AttackPosition) * 70f;
        entity.MoveAndSlide();
    }

    private GlobalTypes.EnemyKillResult DetermineWeaponType(IAttackComponent attackComponent)
    {
        if (attackComponent is MeleeAttackComponent)
            return GlobalTypes.EnemyKillResult.MELEE;
        if (attackComponent is RangeAttackComponent)
            return GlobalTypes.EnemyKillResult.GUN;
        return GlobalTypes.EnemyKillResult.NONE; // Falls vorhanden, sonst anpassen
    }
    
}

