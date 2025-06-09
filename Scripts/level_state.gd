class_name LevelState
extends Node

#signal level_completed
signal level_failed
signal start_round(enemies_to_spawn: Array)

@export var enemy_manager: EnemyManager
@export var score_system: ScoreSystem
@export var timer: Timer
@export var player: Player

var level_number: GlobalTypes.LevelNumber

func _ready() -> void:
	player.connect("player_died", _on_player_died)
	init()

func init():
	pass

func pause_game():
	await get_tree().create_timer(AssaultGlobals.preperation_time).timeout

func _on_enemies_defeated():
	_win("All enemies defeated")
	LevelManager.load_random_level()

func _on_time_out():
	_lose("Time ran out")

func level_completed():
	timer.connect("timeout", _on_time_out)
	LevelManager.load_random_level()
	print("level completed")

func _on_player_died():
	_lose("Player died")

func _win(reason: String):
	print("LEVEL COMPLETE: %s" % reason)
	emit_signal("level_completed")

func _lose(reason: String):
	print("LEVEL FAILED: %s" % reason)
	emit_signal("level_failed")
