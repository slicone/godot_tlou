extends Node

var level_scenes = [
	"res://levels/Level1.tscn",
]

var last_level_path := ""

func load_random_level():
	var available_levels = level_scenes.duplicate()
	available_levels.erase(last_level_path) # aktuelles Level entfernen

	if available_levels.is_empty():
		push_error("No levels left to load!") # Optional: alles wurde schon gespielt
		return

	var next_level = available_levels[randi() % available_levels.size()]
	last_level_path = next_level
	get_tree().change_scene_to_file(next_level)
