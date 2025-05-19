extends CharacterBody2D

# Basis-Gegner mit Gesundheit, Bewegung & Animation

@export var speed = 200 # max speed
@export var acceleration = 70
var player: Node2D

var health = 100

func _ready():
	player = get_tree().get_root().get_node("Main/Player") 
	$AnimationPlayer.play("idle")
	self.register_events()
	

func _physics_process(delta):
	if player:
		var direction: Vector2 = (player.global_position - global_position).normalized()
		var target_velocity = direction * speed
		velocity = velocity.move_toward(target_velocity, acceleration * delta)
		
	move_and_slide()

func register_events() -> void:
	$DetectionArea.connect("body_entered", self._on_detection_entered)
	$DetectionArea.connect("body_exited", self._on_detection_exited)
	$AttackArea.connect("body_entered", self._on_attack_entered)
	$AttackArea.connect("body_exited", self._on_attack_exited)


func _on_detection_entered(body):
	if body.is_in_group("Player"):
		print("Player detected — start chasing!")

func _on_detection_exited(body):
	if body.is_in_group("Player"):
		print("Player lost — stop chasing!")

func _on_attack_entered(body):
	if body.is_in_group("Player"):
		print("Player in attack range — attack now!")

func _on_attack_exited(body):
	if body.is_in_group("Player"):
		print("Player out of attack range — stop attack")

func take_damage(amount):
	health -= amount
	if health <= 0:
		die()

func die():
	queue_free()
