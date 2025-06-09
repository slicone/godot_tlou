class_name ScoreSystem
extends Node

@export
var enemy_manager: EnemyManager
var current_score: int = 0
# represents the values to calculate the score
# TODO maybe different layouts for dependend mode. Or extended classes?
var kill_log = {
	"performance": {
		"encounter_completion": false,
		"time_completed": 0,
		"damage_taken": 0,
		"accuracy": 0
	},
	"bonus": {
		"gambit_complete": false,
		"stuns": 0,
		"stealth_kills": 0,
		"melee_kills": 0,
		"headshot_kills": 0,
		"headshot_only": false,
		"melee_only": false,
		"unseen": false
	}
}

const points_lookup_table = {
	"basic": {
		"performance": {
			"encounter_completion": 100,
			"damage_taken": 100,
			"accuracy": 100
		},
		"bonus": {
			"gambit_complete": 10,
			"stuns": 5,
			"stealth_kills": 5,
			"melee_kills": 5,
			"headshot_only": 50,
			"melee_only": 50,
			"unseen": 50
		}
	},
	"hunted": {
		"performance": {
			"enemies_killed": 10
		}
	},
	"assault": {
		"performance": {
			"time_completed": 100
		}
	},
	"capture": {
		"performance": {
			"time_completed": 100
		},
		"bonus": {
			"safe_captured": 50
		}
	},
	"holdout": {
		"performance": {
			"ally_damage_taken": 100
		},
	}
}
	
func _ready():
	enemy_manager.connect("enemy_died", _add_kill_result)
	
func _add_kill_result(enemy: EnemyBase, kill_result: Array) -> void:
	for result: GlobalTypes.EnemyKillResult in kill_result:
		var kill_log_bonus = kill_log["bonus"]
		match result:
			GlobalTypes.EnemyKillResult.HEADSHOT:
				kill_log_bonus["headshot_kills"] += 1
				break
			GlobalTypes.EnemyKillResult.MELEE:
				kill_log_bonus["melee_kills"] += 1
				break
			GlobalTypes.EnemyKillResult.STUNNED:
				kill_log_bonus["stuns"] += 1
				break
			_:
				print("Unknown kill result")
	
func _calculate_score():
	pass
