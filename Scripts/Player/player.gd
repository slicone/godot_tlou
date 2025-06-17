extends CharacterBody2D
class_name Player

var current_weapon: Weapon = null
@onready 
var animation_player: AnimationPlayer = $AnimationPlayer
@onready
var state_machine = $state_machine
@onready
var weapon_manager = $WeaponManager
@onready
var hitbox_component: HitboxComponent = $HitboxComponent
@onready
var pickup_area: PickupArea = $PickupArea

signal player_died
signal attack
signal interact
signal drop

func _ready() -> void:
	var health = $HealthComponent
	health.connect("entity_died", _player_died)
	hitbox_component.health_component = health
	state_machine.init(self)
	weapon_manager.init(self)

func get_world() -> Node:
	return get_tree().get_current_scene()

# kill_result unused
func _player_died(kill_result: Array) -> void:
	player_died.emit()

func _unhandled_input(event: InputEvent) -> void:
	state_machine.process_input(event)

func _process(delta):
	state_machine.process_frame(delta)

func _physics_process(delta: float) -> void:
	state_machine.process_physics(delta)
