extends State

@export
var idle_state: State
@export
var move_state: State
@export
var fall_state: State

const JUMP_VELOCITY = -300.0

func process_input(event: InputEvent) -> State:
	check_non_state_input()
	return null
	
func process_physics(delta: float) -> State:
	parent.velocity.y = JUMP_VELOCITY
	
	if not parent.is_on_floor():
		return fall_state
	
	var movement = Input.get_axis("move-left", "move-right") * move_speed
	if movement != 0:
		animation.flip_h = movement < 0
	parent.velocity.x = movement
	parent.move_and_slide()
	
	if parent.is_on_floor():
		if movement != 0:
			return move_state
		return idle_state
		
	return null
