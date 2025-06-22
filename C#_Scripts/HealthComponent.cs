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
        CheckIfEntityDies(entity, attack.WeaponType);
    }

    private void CalculateEntityHealth(Attack attack)
    {
        Health -= attack.AttackDamage;
        EmitSignal(SignalName.HealthChanged);
        EmitSignal(SignalName.EntityTookDamage, (int) attack.AttackDamage);
    }

    private void CheckIfEntityDies(CharacterBody2D entity, string weaponType)
    {
        if (Health <= 0)
        {
            var killResult = GetEntityKillResult(entity, weaponType);
            EmitSignal(SignalName.EntityDied, killResult);
            entity.QueueFree();
        }
    }

    
    private Godot.Collections.Array<GlobalTypes.EnemyKillResult> GetEntityKillResult(CharacterBody2D entity, string weaponType)
    {
        var killResult = new Godot.Collections.Array<GlobalTypes.EnemyKillResult>();
        if (entity is EnemyBase enemy)
        {
            killResult.Add(DetermineWeaponType(weaponType));
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
        entity.Velocity = entity.GlobalPosition.DirectionTo(attack.AttackPosition) * 70f;
        entity.MoveAndSlide();
    }

    private GlobalTypes.EnemyKillResult DetermineWeaponType(string weaponType)
    {
        if (weaponType == typeof(MeleeAttackComponent).Name)
            return GlobalTypes.EnemyKillResult.MELEE;
        if (weaponType == typeof(RangeAttackComponent).Name)
            return GlobalTypes.EnemyKillResult.GUN;
        return GlobalTypes.EnemyKillResult.NONE;
    }
    
}

