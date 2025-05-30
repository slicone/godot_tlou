extends CanvasLayer

func _ready() -> void:
	print("hallo")
	get_viewport().size = DisplayServer.screen_get_size()
