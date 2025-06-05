extends TextureProgressBar

var health_component: HealthComponent

func _ready():
	health_component = get_parent().get_node("HealthComponent")
	if not health_component:
		return
	health_component.connect("healthChanged", update)
	update()

func update():
	value = health_component.health * 100 / health_component.MAX_HEALTH
