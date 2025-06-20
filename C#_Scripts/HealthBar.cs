using Godot;
using System;

public partial class HealthBar : TextureProgressBar
{
	[Export]
	public HealthComponent HealthComponent { get; set; }

	public override void _Ready()
	{
		// Falls kein Export gesetzt ist, versuche automatisch das Node zu finden
		if (HealthComponent == null)
		{
			HealthComponent = GetParent()?.GetNode<HealthComponent>("HealthComponent");
		}

		if (HealthComponent == null)
			return;

		HealthComponent.HealthChanged += UpdateHealth;
		UpdateHealth();
	}

	private void UpdateHealth()
	{
		if (HealthComponent == null || HealthComponent.MAX_HEALTH == 0)
			return;

		Value = HealthComponent.Health * 100.0f / HealthComponent.MAX_HEALTH;
	}
}
