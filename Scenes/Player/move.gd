extends State

@export
var idle_state: State
@export
var fall_state: State
@export
var jump_state: State
var time_since_input: float = 0.0

func process_input(event: InputEvent) -> State:
	check_non_state_input()
	if Input.is_action_just_pressed("jump") and parent.is_on_floor():
		return jump_state
	return null
	
func process_physics(delta: float) -> State:
	var movement = Input.get_axis("move-left", "move-right") * move_speed

	if not parent.is_on_floor():
		return fall_state
	
	if movement != 0:
		# TODO make Animation node, where all corresponding nodes will be flipped
		animation.flip_h = movement < 0
		#if (parent.weapon_holder.position.x > 0) or (parent.weapon_holder.position.x < 0):
			#if parent.current_weapon:
				#parent.weapon_holder.position.x *= -1
				#parent.current_weapon.flip_sprite()
		parent.velocity.x = movement
		time_since_input = 0.0
	else:
		parent.velocity.x = move_toward(parent.velocity.x, 0, move_speed)
		if parent.velocity.x == 0:
			time_since_input += delta
		
	if time_since_input > 0.2:
		return idle_state
		
	parent.move_and_slide()
	return null
