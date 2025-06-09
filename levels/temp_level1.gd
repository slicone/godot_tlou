extends Node2D

func _ready() -> void:
	(self.get_node("LevelState") as LevelState).init(GlobalTypes.LevelNumber.FIRST)
