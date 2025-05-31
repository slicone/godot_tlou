extends Area2D
class_name Weapon
@export var attack_damage: float = 20.0

func _ready():
	register_events()

func register_events() -> void:
	self.connect("area_entered", _on_attack_entered)

func _on_attack_entered(area: Area2D):
	if area is HitboxComponent && area.is_in_group("Enemy"):
		var hitbox: HitboxComponent = area
		var attack = Attack.new()
		attack.attack_damage = attack_damage
		hitbox.damage(attack)
