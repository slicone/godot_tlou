class_name RangeAttackComponent
extends AttackComponent

@export var fire_rate := 0.3
var can_fire := true
@onready
var ray_cast = $RayCast2D
@onready
var fire_timer = $FireTimer

func attack():
	weapon.animation_player.play("shoot")
	shoot()
	#weapon.animation_player.stop(true)

func _ready():
	fire_timer.wait_time = fire_rate
	fire_timer.one_shot = true
	ray_cast.collide_with_areas = true
	
func _physics_process(delta: float) -> void:
	var mouse_pos = get_global_mouse_position()
	#weapon.look_at(mouse_pos)
	
func shoot():
	can_fire = false
	fire_timer.start()

	var mouse_pos = get_global_mouse_position()
	var dir = (mouse_pos - global_position).normalized()
	
	ray_cast.target_position = dir * 1000  # Länge des Strahls

	ray_cast.force_raycast_update()

	if ray_cast.is_colliding():
		var hit = ray_cast.get_collider()
		if hit is HitboxComponent:
			var attack = Attack.new()
			attack.attack_damage = 10
			hit.damage(attack)

	# Optional: Sound oder Rückstoß
	#$AudioStreamPlayer2D.play()

func _on_fire_timer_timeout():
	can_fire = true
	
