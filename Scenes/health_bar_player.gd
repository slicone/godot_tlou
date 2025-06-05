extends ProgressBar

@export var player: Player

func _ready():
	var health_component = player.hitbox_component.health_component
	health_component.connect("healthChanged", update)
	update()

func update():
	var health_component = player.hitbox_component.health_component
	value = health_component.health * 100 / health_component.MAX_HEALTH
