extends Node

var level_scenes = [
	"res://levels/Level1.tscn",
]

var LevelNumber := GlobalTypes.LevelNumber
var level_numbers = [LevelNumber.FIRST, LevelNumber.SECOND, LevelNumber.THIRD, LevelNumber.FOURTH, LevelNumber.FIFTH, LevelNumber.SIXTH]
var level_index = 0
var last_level_path := ""

func instantiate_level(level_path: String, level_number: GlobalTypes.LevelNumber):
	var level_scene = load(level_path)
	var level_instance: Node2D = level_scene.instantiate()
	return level_instance

# TODO do i want to call the init or should the level state do it themselve
#func call_level_state_init(level_instance: Node2D, level_path: String, level_number: GlobalTypes.LevelNumber):
	#for child in get_children():
		#if child is LevelState:
			#child.init()
			#return
	#push_error("Level with path %s doesn't have a AssaultLevelState" % level_path)
	
func increment_level_number():
	RunState.level_number = level_numbers[level_index]
	level_index += 1

func change_level(level_path: String, level_number: GlobalTypes.LevelNumber):
	var level_instance = instantiate_level(level_path, level_number)
	# delete old scene
	get_tree().get_current_scene().queue_free()
	# set new scene
	get_tree().root.add_child(level_instance)
	get_tree().current_scene = level_instance
	
	increment_level_number()
	#call_level_state_init(level_instance, level_path, level_number)
	
# TODO will be deprecated in future
func get_random_level_path(available_levels: Array):
	var next_level = available_levels[randi() % available_levels.size()]
	last_level_path = next_level
	return next_level

# TODO generate Level Array
func generate_level_road_map():
	pass

func load_random_level():
	#var available_levels = level_scenes.duplicate() TODO why did chatgpt wanted it to be copied?
	level_scenes.erase(last_level_path)
	if level_scenes.is_empty():
		push_error("No levels left to load!")
		return
	var next_level = get_random_level_path(level_scenes)
	change_level(next_level, GlobalTypes.LevelNumber.FIRST)
