class_name EnemyManager
extends Node
# TODO EnemyManager in Baseklasse und die hier ist ein AssaultEnemyManager()?

@export
var level_state: LevelState
var enemyScene := preload("res://Scenes/Enemies/enemy_base.tscn")
signal all_enemies_defeated
signal enemy_died(enemy: EnemyBase, kill_stats: Array)

func _ready() -> void:
	level_state.connect("start_round", _spawn_enemies)
	
func remove_enemy(enemy: EnemyBase):
	remove_child(enemy)

func _spawn_enemies(number_of_enemies: Array):
	for i in number_of_enemies.size(): # TODO spawn enemies dependent on type
		var enemy_instance = enemyScene.instantiate()
		add_child(enemy_instance)
		enemy_instance.connect("enemy_died", _on_enemy_died)
		enemy_instance.global_position = Vector2(200 + i, 100 + i)
		
func spawn_enemy_dependent_on_type(enemy_type: GlobalTypes.EnemyTypes):
	#TODO
	pass

func _on_enemy_died(enemy: EnemyBase, kill_stats: Array):
	remove_enemy(enemy)
	enemy_died.emit(enemy, kill_stats)
	if get_child_count() == 0:
		emit_signal("all_enemies_defeated")
