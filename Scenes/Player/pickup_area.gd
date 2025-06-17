class_name PickupArea
extends Area2D

signal weapon_nearby_entered(weapon: Weapon)
signal weapon_nearby_exited(weapon: Weapon)

func _ready():
	register_events()

func register_events() -> void:
	self.connect("area_entered", _on_entered)
	self.connect("area_exited", _on_exited)
	
func _on_entered(area: Area2D):
	if area is Weapon:
		var weapon = area as Weapon
		weapon_nearby_entered.emit(weapon)
		
func _on_exited(area: Area2D):
	if area is Weapon:
		var weapon = area as Weapon
		weapon_nearby_exited.emit(weapon)
