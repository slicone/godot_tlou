extends Area2D
class_name HitboxComponent

@export var health_component: HealthComponent

func damage(attack: Attack, attack_component: AttackComponent):
	if health_component:
		health_component.damage(attack, attack_component)
