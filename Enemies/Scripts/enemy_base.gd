extends CharacterBody2D

# Basis-Gegner mit Gesundheit, Bewegung & Animation

@export var speed = 100 # max speed
@export var acceleration = 70
var player: Node2D
var player_detected: bool = false;

# patrol
@export var patrol_points: Array[NodePath]  # Im Inspector einstellbar
var current_target_index = 0
var target_position: Vector2
var direction: Vector2

@export var health: float = 100.0
@export var damage: float = 5.0
@export var patrol_distance = [5, 5] # max left, max right

func _ready():
	player = get_tree().get_root().get_node("Main/Player")
	$AnimationPlayer.play("idle")
	if patrol_points.size() > 0:
		target_position = get_node(patrol_points[current_target_index]).global_position
	self.register_events()
	

func _physics_process(delta):
	print(player)
	if player && player_detected:
		var pos_dif: Vector2 = player.global_position - global_position
		var distance: float = player.global_position.distance_to(global_position)
		var direction: Vector2 = Vector2(pos_dif.x, 0).normalized()
		print(direction)
		#var direction: Vector2 = global_position.direction_to(player.global_position).normalized()
		var target_velocity: Vector2 = direction * speed
		if distance < 50:
			# slow down
			velocity = velocity.move_toward(Vector2(0,0), acceleration / 2 * delta)
		else:
			velocity = velocity.move_toward(target_velocity, acceleration * delta)
	else:
		if patrol_points.size() == 0:
			return

		direction = (target_position - global_position).normalized()
		velocity = direction * speed

		if global_position.distance_to(target_position) < 2:
			current_target_index += 1
			if current_target_index >= patrol_points.size():
				current_target_index = 0  # Zurück zum ersten Punkt
			target_position = get_node(patrol_points[current_target_index]).global_position
		
	move_and_slide()

func register_events() -> void:
	$DetectionArea.connect("body_entered", self._on_detection_entered)
	$DetectionArea.connect("body_exited", self._on_detection_exited)
	$AttackArea.connect("body_entered", self._on_attack_entered)
	$AttackArea.connect("body_exited", self._on_attack_exited)


func _on_detection_entered(body):
	if body.is_in_group("Player"):
		player_detected = true
		print("Player detected — start chasing!")

func _on_detection_exited(body):
	if body.is_in_group("Player"):
		player_detected = false
		print("Player lost — stop chasing!")

func _on_attack_entered(body):
	if body.is_in_group("Player"):
		if body.has_method("take_damage"):
			body.take_damage(damage)

func _on_attack_exited(body):
	if body.is_in_group("Player"):
		print("Player out of attack range — stop attack")

func take_damage(amount):
	health -= amount
	if health <= 0:
		die()

func die():
	queue_free()
