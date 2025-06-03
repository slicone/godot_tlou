extends CharacterBody2D
class_name Player

var weapons_nearby : Array[Weapon] = []
var current_weapon: Weapon = null
@onready 
var world = get_tree().get_current_scene()
@onready 
var weapon_holder = $WeaponHolder
@onready 
var animation_player: AnimationPlayer = $AnimationPlayer
@onready
var state_machine = $state_machine


func _ready() -> void:
	var hitbox = $HitboxComponent
	var health = $HealthComponent as HealthComponent
	health.setHealth(50)
	hitbox.health_component = health
	$Idle.visible = true
	#animation_player = get_node("AnimationPlayer")
	animation_player.play("idle")
	state_machine.init(self)

func _unhandled_input(event: InputEvent) -> void:
	state_machine.process_input(event)

func _process(delta):
	state_machine.process_frame(delta)

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
	state_machine.process_physics(delta)
	

	# Handle jump.
	
	
	
	
