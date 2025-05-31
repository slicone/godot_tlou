extends Area2D

func _ready():
	register_events()

func register_events() -> void:
	self.connect("area_entered", _on_attack_entered)
	self.connect("area_exited", _on_attack_exited)
	
func _on_attack_entered(area: Area2D):
	if area is Weapon:
		var weapon = area as Weapon
		var player = get_parent() as Player
		if weapon not in player.weapons_nearby:
			player.weapons_nearby.append(weapon)
		
func _on_attack_exited(area: Area2D):
	if area is Weapon:
		var weapon = area as Weapon
		var player = get_parent() as Player
		if weapon in player.weapons_nearby:
			player.weapons_nearby.erase(weapon)
