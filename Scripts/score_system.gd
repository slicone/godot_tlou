class_name ScoreSystem
extends Node

@export
var enemy_manager: EnemyManager
var current_score: int = 0
var round_time: int

# represents the values to calculate the score
var encounter_stats_static = {
	# Only tracks shared stats across all modes
	# Add key-value for independent mode stats
	"performance": {
		"encounter_completion": false,
	},
	"bonus": {
		"gambit_complete": false, # TODO need gambit system for that
		"stuns": 0, # TODO stun logic for enemy missing
		"stealth_kills": 0, # TODO complex because there needs to be an system that decides if player was detected
		"melee_kills": 0,
		"headshot_kills": 0,
		"headshot_only": false,
		"melee_only": false,
		"unseen": false # TODO macht das sinn bei 2d?
	},
	"tracking_only": {
		"gun_kills": 0,
		"gadget_kills": 0 # TODO is that even neccessary?
	},
}
var encounter_stats_dynamic = {
	"performance": {
		"damage_taken": 0,
		"accuracy": 0
	}
}

const points_lookup_table_static = {
	"performance": {
		"encounter_completion": 100,
		"enemies_killed": 10,
	},
	"bonus": {
		"gambit_complete": 10,
		"stuns": 5,
		"stealth_kills": 5,
		"melee_kills": 5,
		"headshot_only": 50,
		"melee_only": 50,
		"unseen": 50,
		"safe_captured": 50
	},
}

const points_lookup_table_dynamic = {
	"performance": {
		"damage_taken": 100,
		"accuracy": 100,
		"time_completed": 100,
		"ally_damage_taken": 100
	}
}

func _ready():
	enemy_manager.connect("enemy_died", _add_kill_result)

func _add_kill_result(enemy: EnemyBase, kill_result: Array) -> void:
	for result: GlobalTypes.EnemyKillResult in kill_result:
		var encounter_log_bonus = encounter_stats_static["bonus"]
		var encounter_track_only = encounter_stats_static["tracking_only"]
		match result:
			GlobalTypes.EnemyKillResult.HEADSHOT:
				encounter_log_bonus["headshot_kills"] += 1
				break
			GlobalTypes.EnemyKillResult.MELEE:
				encounter_log_bonus["melee_kills"] += 1
				break
			GlobalTypes.EnemyKillResult.STUNNED:
				encounter_log_bonus["stuns"] += 1
				break
			GlobalTypes.EnemyKillResult.STEALTH:
				encounter_log_bonus["stealth_kills"] += 1
				break
			GlobalTypes.EnemyKillResult.GUN:
				encounter_track_only["gun_kills"] += 1
				break
			_:
				push_error("Unknown kill result")

# collect points from damage_taken and accuracy because they are used in every mode
# for mode specific dynamics stats use seperate method
func collect_points_from_dynamic_default_stats() -> int:
	# TODO calculate damage and accuracy
	return 0

func collect_points_from_static_stats(stats: Dictionary, points: Dictionary, key_path := [], total := 0) -> int:
	for key in stats.keys():
		var value = stats[key]
		var new_path = key_path + [key]

		if typeof(value) == TYPE_DICTIONARY:
			total = collect_points_from_static_stats(value, points, new_path, total)
		else:
			if not value:
				continue # skip if false or 0
			if typeof(value) == TYPE_BOOL:
				value = 1 # at this point bool value can only be true
			var point_value = get_value_at_path(points, new_path, value)
			if typeof(point_value) in [TYPE_INT, TYPE_FLOAT]:
				total += point_value
	return total

func get_value_at_path(dict: Dictionary, path: Array, count: int):
	var current = dict
	for i in range(path.size()):
		if current.has(path[i]):
			current = current[path[i]]
		else:
			return null
	return current * count

func set_round_timer(round_time: int):
	self.round_time = round_time

func set_encounter_value(value: bool):
	encounter_stats_static["performance"]["encounter_completion"] = value

func get_score():
	return current_score
	
func get_encounter_stats():
	return [encounter_stats_static, encounter_stats_dynamic]

func is_meele_only():
	encounter_stats_static["bonus"]["melee_only"] = encounter_stats_static["tracking_only"]["gun_kills"] == 0

func set_player_damage(damage: int):
	pass

func is_headshot_only():
	pass

func calculate_score():
	push_error("Needs to be implemented by child class")
	pass
# calculate score for dynamic mode specific stats
func collect_points_from_dynamics_mode_stats():
	push_error("Needs to be implemented by child class")
	pass
