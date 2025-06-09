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
	
	if not level_instance.has_node("LevelState"):
		push_error("Level with path %s doesn't have a LevelState" % level_path)
	var level_state: LevelState = level_instance.get_node("LevelState")
	level_state.init(level_number)
	
	return level_instance

func change_level(level_instance: Node2D):
	# delete old scene
	get_tree().get_current_scene().queue_free()
	# set new scene
	get_tree().root.add_child(level_instance)
	get_tree().current_scene = level_instance
	level_index += 1

# TODO will be deprecated in future
func get_random_level_path(available_levels: Array):
	available_levels.erase(last_level_path)
	var next_level = available_levels[randi() % available_levels.size()]
	last_level_path = next_level
	return next_level

# TODO generate Level Array
func generate_level_road_map():
	pass

func load_random_level():
	var available_levels = level_scenes.duplicate()
	if available_levels.is_empty():
		push_error("No levels left to load!")
		return
	var next_level = get_random_level_path(available_levels)
	var level_instance = instantiate_level(next_level, GlobalTypes.LevelNumber.FIRST) # TODO change level_number	
	change_level(level_instance)
