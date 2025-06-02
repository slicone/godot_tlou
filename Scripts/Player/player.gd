extends CharacterBody2D
class_name Player

const SPEED = 300.0
const JUMP_VELOCITY = -350.0
var weapons_nearby : Array[Weapon] = []
var current_weapon: Weapon = null
@onready var weapon_holder = $WeaponHolder
@onready var world = get_tree().get_current_scene()
var animation_player: AnimationPlayer
var facing_left: bool = false

func _ready() -> void:
	var hitbox = $HitboxComponent
	var health = $HealthComponent as HealthComponent
	health.setHealth(50)
	hitbox.health_component = health
	$Idle.visible = true
	animation_player = get_node("AnimationPlayer")
	animation_player.play("idle")

func _process(delta):
	if Input.is_action_just_pressed("interact"):
		pick_up_weapon()
	if Input.is_action_just_pressed("drop"):
		drop_current_weapon()
	if Input.is_action_just_pressed("attack"):
		if current_weapon and current_weapon.has_method("attack"):
			current_weapon.attack()

func pick_nearest_weapon() -> Weapon:
	var shortest_distance = INF
	var closest_weapon = null
	for weapon in weapons_nearby:
		if weapon == current_weapon:
			continue
		var distance = weapon.global_position.distance_to(global_position)
		if distance < shortest_distance:
			shortest_distance = distance
			closest_weapon = weapon
	return closest_weapon

func drop_current_weapon():
	if current_weapon:
		weapon_holder.remove_child(current_weapon)
		world.add_child(current_weapon)
		current_weapon.position = global_position
		current_weapon.gravity_enabled = true
		current_weapon = null
		
func pick_up_weapon():
	var weapon = pick_nearest_weapon()
	if not weapon:
		return
	drop_current_weapon()
	if weapon.get_parent():
		weapon.get_parent().remove_child(weapon)
	weapon_holder.add_child(weapon)
	weapon.position = Vector2.ZERO # otherwise the position to the main scene is transferred to the player
	weapon.gravity_enabled = false
	current_weapon = weapon

func _physics_process(delta: float) -> void:
	if not is_on_floor():
		velocity += get_gravity() * delta

	# Handle jump.
	if Input.is_action_just_pressed("ui_accept") and is_on_floor():
		velocity.y = JUMP_VELOCITY

	var direction := Input.get_axis("ui_left", "ui_right")
	
	$Run.visible = direction != 0.0
	$Idle.visible = direction == 0.0
	if direction:
		facing_left = direction < 0
		$Run.flip_h = facing_left
		if (facing_left and $WeaponHolder.position.x > 0) or (not facing_left and $WeaponHolder.position.x < 0):
			if current_weapon:
				$WeaponHolder.position.x *= -1
				current_weapon.flip_sprite()

		
		animation_player.play("run")
		velocity.x = direction * SPEED
	else:
		$Idle.flip_h = facing_left
		animation_player.play("idle")
		velocity.x = move_toward(velocity.x, 0, SPEED)
		
	move_and_slide()
	
