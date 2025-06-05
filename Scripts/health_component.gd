extends Node2D
class_name HealthComponent

@export var MAX_HEALTH := 100.0
@export
var health: float
signal healthChanged
signal entity_died

func _ready():
	health = MAX_HEALTH
	
func damage(attack: Attack):
	health -= attack.attack_damage
	healthChanged.emit()
	if health <= 0:
		emit_signal("entity_died")
		get_parent().queue_free()
	
