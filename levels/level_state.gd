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
var current_assault_round = 0
var assault_lookup: Array
var assault_lookup_index: int

func _ready():
	enemy_manager.connect("all_enemies_defeated", _assualt_round_finished)
	player.connect("player_died", _on_player_died)
	
func init(level_number: GlobalTypes.LevelNumber):
	self.level_number = level_number
	var template = "template_1"
	assault_lookup = AssaultGlobals.enemies_per_round_template[level_number][template]
	if not assault_lookup:
		push_error("Error. No assault spawn template found for level_number %s and template string %s " % [level_number, template])
	start_assault_round()

func pause_game():
	await get_tree().create_timer(AssaultGlobals.preperation_time).timeout

func _assualt_round_finished():
	current_assault_round += 1
	if current_assault_round >= AssaultGlobals.assault_round_max:
		level_completed()
		return
	await pause_game()
	start_round.emit(get_assault_lookup_round_value())
	
func get_assault_lookup_round_value():
	if not assault_lookup:
		push_error("Can't increment assault_lookup in LevelState because it's null or empty")
		return
	if assault_lookup_index >= len(assault_lookup):
		push_error("Can't increment assault_lookup in LevelState because index out of range")
		return
	var assault_template = assault_lookup[assault_lookup_index]
	assault_lookup_index += 1
	return assault_template
	
func start_assault_round():
	await pause_game()
	start_round.emit(get_assault_lookup_round_value())
	# TODO nach den 20sec oder davor? bzw muss er kurzzeitig gestoppt oder einfach wartezeit abziehen
	timer.start()

func _on_enemies_defeated():
	_win("All enemies defeated")
	LevelManager.load_random_level()

func _on_time_out():
	_lose("Time ran out")

func level_completed():
	timer.connect("timeout", _on_time_out)
	print("level completed")

func _on_player_died():
	_lose("Player died")

func _win(reason: String):
	print("LEVEL COMPLETE: %s" % reason)
	emit_signal("level_completed")

func _lose(reason: String):
	print("LEVEL FAILED: %s" % reason)
	emit_signal("level_failed")
