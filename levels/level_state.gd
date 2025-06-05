class_name LevelState
extends Node

signal level_completed
signal level_failed

@export var required_score: int = 100
@export var enemy_manager: EnemyManager
@export var score_system: ScoreSystem
@export var timer: Timer
@export var player: Player
var current_score: int = 0

func _ready():
	enemy_manager.connect("all_enemies_defeated", _on_enemies_defeated)
	timer.connect("timeout", _on_time_out)
	player.connect("player_died", _on_player_died)

func add_score(points: int):
	current_score += points
	if required_score > 0 and current_score >= required_score:
		_win("Score reached")

func _on_enemies_defeated():
	_win("All enemies defeated")
	LevelManager.load_random_level()

func _on_time_out():
	_lose("Time ran out")

func _on_player_died():
	_lose("Player died")

func _win(reason: String):
	print("LEVEL COMPLETE: %s" % reason)
	emit_signal("level_completed")

func _lose(reason: String):
	print("LEVEL FAILED: %s" % reason)
	emit_signal("level_failed")
