extends CharacterBody2D

@export var speed = 100 # max speed
@export var acceleration = 70
@export var attack_damage: float = 5.0
@export var health: float = 20.0
var player: Node2D
var weapon: Weapon
var player_detected: bool = false;

# patrol
@export var patrol_points: Array[NodePath]  # Im Inspector einstellbar
var current_target_index = 0
var target_position: Vector2

func setup_character():
	player = get_tree().get_root().get_node("Main/Player")
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
	if player && player_detected:
		# target_pos should change with enemy_pos
		target_position = get_node(patrol_points[current_target_index]).global_position
		var pos_dif: Vector2 = player.global_position - global_position
		var distance: float = player.global_position.distance_to(global_position)
		var direction: Vector2 = Vector2(pos_dif.x, 0).normalized()
		#var direction: Vector2 = global_position.direction_to(player.global_position).normalized()
		var target_velocity: Vector2 = direction * speed
		if distance < 50:
			# slow down
			velocity = velocity.move_toward(Vector2.ZERO, acceleration / 2 * delta)
		else:
			velocity = velocity.move_toward(target_velocity, acceleration * delta)
	else:
		if patrol_points.size() == 0:
			return
		var direction = (target_position - global_position).normalized()
		velocity = direction * speed
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
