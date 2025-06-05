class_name EnemyManager
extends Node

var enemyScene := preload("res://Scenes/Enemies/enemy_base.tscn")
signal all_enemies_defeated

func _ready() -> void:
	var enemy_instance = enemyScene.instantiate()
	add_child(enemy_instance)
	enemy_instance.connect("enemy_died", _on_enemy_died)
	enemy_instance.global_position = Vector2(200, 100)

func remove_enemy(enemy: EnemyBase):
	remove_child(enemy)
	
func _on_enemy_died(enemy: EnemyBase, kill_stats: Array):
	remove_enemy(enemy)
	if get_child_count() == 0:
		emit_signal("all_enemies_defeated")
