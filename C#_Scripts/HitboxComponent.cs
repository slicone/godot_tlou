using Godot;

public partial class HitboxComponent : Area2D
{

	[Export] public HealthComponent HealthComponent { get; set; }


    public override void _Ready()
    {
		//HealthComponent = GetNodeOrNull<HealthComponent>("HealthComponent");
		if (HealthComponent is null)
		{
			GD.PushError("HitboxComponent doesn't have HealthComponent dependency");
		}
    }

	public void Damage(Attack attack)
	{
		HealthComponent?.Damage(attack);
	}
}
