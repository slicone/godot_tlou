extends Area2D
class_name AttackComponent
@export
var weapon: Weapon

func attack():
	push_error("Only an Interface. Override this function")
