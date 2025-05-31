extends CharacterBody2D
class_name Player

const SPEED = 300.0
const JUMP_VELOCITY = -400.0
var weapons_nearby : Array[Weapon] = []
var current_weapon: Weapon = null
@onready var weapon_holder = $WeaponHolder

func _ready() -> void:
	var hitbox = $HitboxComponent
	var health = $HealthComponent as HealthComponent
	health.setHealth(50)
	hitbox.health_component = health

func _process(delta):
	if Input.is_action_just_pressed("interact"):
		pick_up_weapon()

func pick_up_weapon():
	if weapons_nearby.is_empty():
		return
	var weapon = weapons_nearby.get(0)
	if current_weapon:
		weapon_holder.remove_child(current_weapon)
	if weapon.get_parent():
		weapon.get_parent().remove_child(weapon)
	weapon_holder.add_child(weapon)
	print(weapon_holder.get_children())
	weapon.position = Vector2.ZERO # otherwise the position to the main scene is transferred to the player

func _physics_process(delta: float) -> void:
	if not is_on_floor():
		velocity += get_gravity() * delta

	# Handle jump.
	if Input.is_action_just_pressed("ui_accept") and is_on_floor():
		velocity.y = JUMP_VELOCITY

	# Get the input direction and handle the movement/deceleration.
	# As good practice, you should replace UI actions with custom gameplay actions.
	var direction := Input.get_axis("ui_left", "ui_right")
	if direction:
		velocity.x = direction * SPEED
	else:
		velocity.x = move_toward(velocity.x, 0, SPEED)
		
	move_and_slide()
	
