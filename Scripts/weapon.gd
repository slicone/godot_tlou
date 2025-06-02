extends Area2D
class_name Weapon
@export var weapon_attack_component: AttackComponent

var velocity = Vector2.ZERO
var gravity_enabled = true
var on_floor = false

func contains_static_body(arr: Array) -> bool:
	for item in arr:
		if item is StaticBody2D:
			return true
	return false

func _physics_process(delta: float) -> void:
	if not gravity_enabled:
		return
	if contains_static_body(get_overlapping_bodies()):
		velocity = Vector2.ZERO
		on_floor = true
		return
	velocity.y += gravity * delta
	position += velocity * delta

func flip_sprite():
	for child in get_children():
		if child is Sprite2D:
			child.flip_h = !child.flip_h

func attack():
	if weapon_attack_component:
		weapon_attack_component.attack()
	
