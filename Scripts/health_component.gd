extends Node2D
class_name HealthComponent

@export var MAX_HEALTH := 100.0
@export
var health: float
signal healthChanged # for progress bar
signal entity_died(kill_result: Array) # Array is GlobalTypes.EnemyKillResult
signal entity_took_damage(damage: int) # if damage is needed

func _ready():
	health = MAX_HEALTH
	
func damage(attack: Attack):
	if not get_parent() is CharacterBody2D:
		push_error("HealtComponent needs a CharacterBody2D as parent")
		return
	var entity: CharacterBody2D = get_parent()
	force_entity_knockback(attack, entity)
	calculate_entity_health(attack)
	check_if_entity_dies(entity, attack.attack_component)

func calculate_entity_health(attack: Attack):
	health -= attack.attack_damage
	healthChanged.emit()
	entity_took_damage.emit(attack.attack_damage)

func check_if_entity_dies(entity: CharacterBody2D, attack_component: AttackComponent):
	if health <= 0:
		emit_signal("entity_died", get_entity_kill_result(entity, attack_component))
		entity.queue_free()

func get_entity_kill_result(entity: CharacterBody2D, attack_component: AttackComponent) -> Array:
	var kill_result = []
	if entity is EnemyBase:
		kill_result.append(determine_weapon_type(attack_component))
		var entity_kill_results = entity.kill_results
		for i in entity_kill_results.size():
			kill_result.append(entity_kill_results[i])
	return kill_result

func force_entity_knockback(attack: Attack, entity: CharacterBody2D):
	# TODO check welche direction
	entity.velocity = entity.global_position.direction_to(attack.attack_position) * 70
	#entity.velocity =  attack.attack_position.direction_to(entity.global_position) * 70
	entity.move_and_slide()
		
func determine_weapon_type(attack_component: AttackComponent):
	if attack_component is MeleeAttackComponent:
		return GlobalTypes.EnemyKillResult.MELEE
	if attack_component is RangeAttackComponent:
		return GlobalTypes.EnemyKillResult.GUN
	
