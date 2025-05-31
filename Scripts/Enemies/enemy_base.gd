extends CharacterBody2D

@export var speed = 150
@export var acceleration = 70
@export var attack_damage: float = 5.0
@export var health: float = 20.0
@export var player_current_scene_path = "Player"
const JUMP_VELOCITY = -400.0
var player: Player
var weapon: Weapon
var player_detected: bool = false;

var last_action_time := 0.0
var cooldown := 1.5  # Sekunden

# patrol
@export var patrol_points: Array[NodePath]
var current_target_index = 0
var target_position: Vector2

func setup_character():
	player = get_tree().get_current_scene().get_node(player_current_scene_path) as Player
	var hitbox = $HitboxComponent
	var health_comp = $HealthComponent as HealthComponent
	health_comp.setHealth(health)
	hitbox.health_component = health_comp

func _ready():
	setup_character()
	if patrol_points.size() > 0:
		target_position = get_node(patrol_points[current_target_index]).global_position
	register_events()
	
# TODO auslagern als Komponente?
func _physics_process(delta):
	if not is_on_floor():
		velocity += get_gravity() * delta
	# Enemy Chase State TODO auslagern als Statemachine
	if player && player_detected:
		# target_pos should change with enemy_pos
		target_position = get_node(patrol_points[current_target_index]).global_position
		# Jump State TODO auslagern als Statemachine
		if not player.is_on_floor() and Time.get_ticks_msec() / 1000.0 - last_action_time >= cooldown:
			velocity.y += JUMP_VELOCITY
			last_action_time = Time.get_ticks_msec() / 1000.0
		var distance: float = player.global_position.distance_to(global_position)
		var direction: Vector2 = global_position.direction_to(player.global_position).normalized()
		var target_velocity: Vector2 = direction * speed
		if distance < 50:
			# slow down
			velocity = velocity.move_toward(Vector2.ZERO, acceleration / 2 * delta)
		else:
			velocity = velocity.move_toward(target_velocity, acceleration * delta)
	# Enemy Idle State TODO auslagern als Statemachine
	else:
		if patrol_points.size() == 0:
			return
		var direction: Vector2 = global_position.direction_to(player.global_position).normalized()
		var target_velocity: Vector2 = direction * speed
		velocity = velocity.move_toward(target_velocity, acceleration * delta)
		if global_position.distance_to(target_position) < 2:
			current_target_index += 1
			if current_target_index >= patrol_points.size():
				current_target_index = 0
			target_position = get_node(patrol_points[current_target_index]).global_position
	move_and_slide()

func register_events() -> void:
	$DetectionAreaComponent.connect("body_entered", self._on_detection_entered)
	$DetectionAreaComponent.connect("body_exited", self._on_detection_exited)
	$AttackAreaComponent.connect("area_entered", self._on_attack_entered)

func _on_attack_entered(area):
	if area is HitboxComponent:
		var hitbox: HitboxComponent = area
		var attack = Attack.new()
		attack.attack_damage = attack_damage
		hitbox.damage(attack)

func _on_detection_entered(body):
	if body.is_in_group("Player"):
		player_detected = true

func _on_detection_exited(body):
	if body.is_in_group("Player"):
		player_detected = false
