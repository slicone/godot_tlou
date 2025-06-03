extends State


			
var weapons_nearby : Array[Weapon] = []
var current_weapon: Weapon = null
@export
var fall_state: State
@export
var jump_state: State
@export
var move_state: State
@export
var idle_state: State

func pick_nearest_weapon() -> Weapon:
	var shortest_distance = INF
	var closest_weapon = null

	for weapon in weapons_nearby:
		if weapon == current_weapon:
			continue
		var distance = weapon.global_position.distance_to(parent.global_position)
		if distance < shortest_distance:
			shortest_distance = distance
			closest_weapon = weapon
	return closest_weapon

func drop_current_weapon():
	if current_weapon:
		parent.weapon_holder.remove_child(current_weapon)
		parent.world.add_child(current_weapon)
		current_weapon.position = parent.global_position
		current_weapon.gravity_enabled = true
		current_weapon = null
		
func pick_up_weapon():
	var weapon = pick_nearest_weapon()
	if not weapon:
		return
	drop_current_weapon()
	if weapon.get_parent():
		weapon.get_parent().remove_child(weapon)
	parent.weapon_holder.add_child(weapon)
	weapon.position = Vector2.ZERO # otherwise the position to the main scene is transferred to the player
	weapon.gravity_enabled = false
	current_weapon = weapon


func process_input(event: InputEvent) -> State:
	if Input.is_action_just_pressed("drop"):
		return drop_current_weapon()
	if Input.is_action_just_pressed("attack"):
		if current_weapon and current_weapon.has_method("attack"):
			current_weapon.attack()
	return null

func process_frame(delta: float) -> State:
	return null
	
func process_physics(delta: float) -> State:
	return null
