using Godot;

public partial class HitboxComponent : Area2D
{

	[Export] public HealthComponent HealthComponent { get; set; }

	public void Damage(Attack attack)
	{
		HealthComponent?.Damage(attack);    
	}
}
