class_name RangeAttackComponent
extends AttackComponent
# TODO Upgrades, maybe a list?

@export var fire_rate := 0.5
@export var reload_time := 2
var can_fire := true
@onready
var ray_cast = $RayCast2D
@onready
var fire_timer = $FireTimer
@onready
var reload_timer = $ReloadTimer

@export var max_magazine_rounds = 6
var current_magazine_rounds = max_magazine_rounds

func attack():
	if can_fire:
		weapon.animation_player.play("shoot")
		shoot()

func _process(delta):
	if Input.is_action_just_pressed("reload") :
		reload()

func _ready():
	setup_timers()
	ray_cast.collide_with_areas = true

func setup_timers():
	fire_timer.wait_time = fire_rate
	reload_timer.wait_time = reload_time
	fire_timer.one_shot = true
	reload_timer.one_shot = true
	fire_timer.connect("timeout", _on_fire_timer_timeout)
	reload_timer.connect("timeout", _on_reload_timer_timeout)
	
#func _physics_process(delta: float) -> void:
	#var mouse_pos = get_global_mouse_position()
	
func shoot():
	can_fire = false
	# check magazine
	if current_magazine_rounds <= 0:
		return
	current_magazine_rounds -= 1
	fire_timer.start()

	# direction
	var mouse_pos = get_global_mouse_position()
	var dir = (mouse_pos - global_position).normalized()
	
	# update raycast collision
	ray_cast.target_position = dir * 1000  # Länge des Strahls
	ray_cast.force_raycast_update()

	if ray_cast.is_colliding():
		var hit = ray_cast.get_collider()
		if hit is HitboxComponent:
			var attack = Attack.new()
			attack.attack_damage = attack_damage
			attack.attack_position = global_position
			attack.knockback_force = 5000
			attack.attack_component = self
			hit.damage(attack)

func reload():
	can_fire = false
	reload_timer.start()

func _on_fire_timer_timeout():
	can_fire = true
	
func _on_reload_timer_timeout():
	current_magazine_rounds = max_magazine_rounds
	can_fire = true
	
