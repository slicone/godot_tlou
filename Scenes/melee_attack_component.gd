class_name MeleeAttackComponent
extends AttackComponent
@export var attack_damage: float = 20.0

func _ready():
	register_events()
	
func register_events() -> void:
	self.connect("area_entered", self._on_attack_entered)

func _on_attack_entered(area):
	if area is HitboxComponent and area.is_in_group("Enemy"):
		var hitbox: HitboxComponent = area
		var attack = Attack.new()
		attack.attack_damage = attack_damage
		hitbox.damage(attack)

func attack():
	weapon.animation_player.play("slash")
	await weapon.animation_player.animation_finished
