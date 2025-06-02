extends AttackComponent
@export var attack_damage: float = 20.0
const number_of_frames_animation: int = 3
var weapon: Weapon

func set_instance_to_weapon():
	var parent = get_parent()
	if parent is Weapon:
		weapon = parent as Weapon
		weapon.weapon_attack_component = self

func _ready():
	set_instance_to_weapon()

func attack():
	var areas = self.get_overlapping_areas()
	for area in areas:
		if area is HitboxComponent and area.is_in_group("Enemy"):
			var hitbox: HitboxComponent = area
			var attack = Attack.new()
			attack.attack_damage = attack_damage
			hitbox.damage(attack)
	(get_parent().get_node("AnimationPlayer") as AnimationPlayer).play("Attack")
	await (get_parent().get_node("AnimationPlayer") as AnimationPlayer).animation_finished
	# duration of animation
	#for i in range(number_of_frames_animation):
	#	await get_tree().process_frame
