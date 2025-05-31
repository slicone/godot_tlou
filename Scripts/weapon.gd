extends Area2D
class_name Weapon

@export var attack_damage: float = 20.0
var velocity = Vector2.ZERO
var gravity_enabled = true
var on_floor = false

func _ready():
	register_events()

func register_events() -> void:
	self.connect("area_entered", _on_attack_entered)

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
	
func _on_attack_entered(area: Area2D):
	if area is HitboxComponent && area.is_in_group("Enemy"):
		var hitbox: HitboxComponent = area
		var attack = Attack.new()
		attack.attack_damage = attack_damage
		hitbox.damage(attack)
