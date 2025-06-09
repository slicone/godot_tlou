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
	if not get_parent() is CharacterBody2D:
		return
	var entity: CharacterBody2D = get_parent()
	health -= attack.attack_damage
	healthChanged.emit()
	# TODO check welche direction
	entity.velocity = entity.global_position.direction_to(attack.attack_position) * 70
	#entity.velocity =  attack.attack_position.direction_to(entity.global_position) * 70
	entity.move_and_slide()
	if health <= 0:
		emit_signal("entity_died")
		entity.queue_free()
	
