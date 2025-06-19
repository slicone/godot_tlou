class_name LevelState
extends Node

#signal level_completed
signal level_failed
signal start_round(enemies_to_spawn: Array) # signal for enemymanager to spawn enemies

@export var enemy_manager: EnemyManager
@export var score_system: ScoreSystem
@export var player: Player
var start_time := 0
var elapsed_time := 0
var level_number: GlobalTypes.LevelNumber

func _ready() -> void:
	player.connect("player_died", _on_player_died)
	init()

func init():
	pass

func pause_game():
	await get_tree().create_timer(GlobalTypes.preperation_time_in_sec).timeout

func _on_enemies_defeated():
	_win("All enemies defeated")
	LevelManager.load_random_level()

func _on_time_out():
	_lose("Time ran out")

func level_completed():
	elapsed_time = Time.get_ticks_msec() / 1000 - start_time
	# prep time not included in round time
	var round_time = elapsed_time - 3 * GlobalTypes.preperation_time_in_sec
	score_system.set_encounter_value(true)
	score_system.set_round_timer(round_time)
	score_system.calculate_score()
	print(score_system.get_score())
	
	var encounter_stats_static = JSON.stringify(score_system.get_encounter_stats()[0], "\t")
	var encounter_stats_dynamic = JSON.stringify(score_system.get_encounter_stats()[1], "\t")
	print("static: \n",  encounter_stats_static)
	print("dynamic: \n",  encounter_stats_dynamic)
	LevelManager.load_random_level()
	print("level completed")

func _on_player_died():
	score_system.set_encounter_value(false)
	score_system.calculate_score()
	_lose("Player died")

func _win(reason: String):
	print("LEVEL COMPLETE: %s" % reason)
	emit_signal("level_completed")

func _lose(reason: String):
	print("LEVEL FAILED: %s" % reason)
	emit_signal("level_failed")
