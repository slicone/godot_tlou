class_name AssaultLevelState
extends LevelState

var current_assault_round = 0
var assault_lookup: Array
var assault_lookup_index: int

func _ready():
	enemy_manager.connect("all_enemies_defeated", _assualt_round_finished)
	super()
	
func init():
	self.level_number = RunState.level_number
	var template = "template_1" # TODO make logic that gets template
	assault_lookup = AssaultGlobals.enemies_per_round_template[level_number][template]
	if not assault_lookup:
		push_error("Error. No assault spawn template found for level_number %s and template string %s " % [level_number, template])
	start_assault_round()

func select_template():
	#TODO
	pass

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
	start_time = Time.get_ticks_msec() / 1000 # seconds
