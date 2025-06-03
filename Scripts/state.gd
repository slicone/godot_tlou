extends Node
class_name State

@export
var animation_name: String
@export
var animation: Sprite2D
@export
var move_speed: float = 300

var gravity

var parent: Player

func enter() -> void:
	parent.animation_player.play(animation_name)
	animation.visible = true

func exit() -> void:
	animation.visible = false
	
func process_input(event: InputEvent) -> State:
	return null

func process_frame(delta: float) -> State:
	return null
	
func process_physics(delta: float) -> State:
	return null
	
func check_non_state_input() -> void:
	if Input.is_action_just_pressed("interact"):
		return parent.pick_up_weapon()
	if Input.is_action_just_pressed("drop"):
		return parent.drop_current_weapon()
	if Input.is_action_just_pressed("attack"):
		if parent.current_weapon and parent.current_weapon.has_method("attack"):
			parent.current_weapon.attack()
