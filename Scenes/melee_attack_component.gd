class_name MeleeAttackComponent
extends AttackComponent
@export var attack_damage: float = 20.0

func _ready():
	register_events()
	
func register_events() -> void:
	self.connect("area_entered", self._on_attack_entered)

func _on_attack_entered(area):
	if area is HitboxComponent:
		var hitbox: HitboxComponent = area
		var attack = Attack.new()
		attack.attack_position = global_position
		attack.knockback_force = 20
		attack.attack_damage = attack_damage
		hitbox.damage(attack, self)

func attack():
	weapon.animation_player.play("slash")
	await weapon.animation_player.animation_finished
