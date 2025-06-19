class_name WeaponManager
extends Node2D

var player: Player
var current_weapon
var weapons_nearby : Array[Weapon] = []
@onready 
var weapon_holder = $WeaponHolder
@export
var item_event_dispatcher: Node

func init(parent: Player):
	player = parent
	player.connect("attack", _attack)
	player.connect("drop", _drop_current_weapon)
	player.connect("interact", _pick_up_weapon)
	item_event_dispatcher.connect("WeaponEntered", _weapon_nearby_entered)
	#player.pickup_area.connect("ItemNearbyExited", _weapon_nearby_exited)

func _weapon_nearby_entered(weapon: Weapon):
	if weapon not in weapons_nearby:
			weapons_nearby.append(weapon)
			
func _weapon_nearby_exited(weapon: Weapon):
	if weapon in weapons_nearby:
		weapons_nearby.erase(weapon)

func _attack():
	if current_weapon and current_weapon.has_method("attack"):
		current_weapon.attack()

func pick_nearest_weapon() -> Weapon:
	var shortest_distance = INF
	var closest_weapon = null
	for weapon in weapons_nearby:
		if weapon == current_weapon:
			continue
		var distance = weapon.global_position.distance_to(player.global_position)
		if distance < shortest_distance:
			shortest_distance = distance
			closest_weapon = weapon
	return closest_weapon

func _drop_current_weapon():
	if current_weapon:
		weapon_holder.remove_child(current_weapon)
		player.get_world().add_child(current_weapon)
		current_weapon.position = player.global_position
		current_weapon.gravity_enabled = true
		current_weapon = null
		
func _pick_up_weapon():
	var weapon = pick_nearest_weapon()
	if not weapon:
		return
	_drop_current_weapon()
	if weapon.get_parent():
		weapon.get_parent().remove_child(weapon)
	weapon_holder.add_child(weapon)
	weapon.position = Vector2.ZERO # otherwise the position to the main scene is transferred to the player
	weapon.gravity_enabled = false
	current_weapon = weapon

# TODO
func setAnimation(animation: String):
	pass
