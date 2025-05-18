extends CharacterBody2D

# Basis-Gegner mit Gesundheit, Bewegung & Animation

@export var speed = 100
var health = 100

func _ready():
	$AnimationPlayer.play("idle")

func _physics_process(delta):
	var window_size = get_viewport_rect().size
	position.x = clamp(position.x, 0, window_size.x)

	# einfache horizontale Bewegung (hin und her)
	velocity.x = speed
	move_and_slide()

func take_damage(amount):
	health -= amount
	if health <= 0:
		die()

func die():
	queue_free()
