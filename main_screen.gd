extends Control


func _on_play_pressed() -> void:
	LevelManager.load_random_level()


func _on_exit_pressed() -> void:
	get_tree().quit()
