extends Area2D
class_name AttackComponent
@export
var weapon: Weapon
@export var attack_damage: float = 20.0

func attack():
	push_error("Only an Interface. Override this function")
