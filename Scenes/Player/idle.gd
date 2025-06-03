extends State

@export
var fall_state: State
@export
var jump_state: State
@export
var move_state: State

func enter() -> void:
	super()

func process_input(event: InputEvent) -> State:
	check_non_state_input()
	if Input.is_action_just_pressed("jump") and parent.is_on_floor():
		return jump_state
	if Input.is_action_just_pressed("move-left") or Input.is_action_just_pressed("move-right"):
		return move_state
	return null
	
func process_physics(delta: float) -> State:
	if not parent.is_on_floor():
		return fall_state
	var movement = Input.get_axis("move-left", "move-right") * move_speed
	if movement != 0:
		return move_state
	return null
