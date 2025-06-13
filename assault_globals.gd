extends Node

var LevelNumber := GlobalTypes.LevelNumber
var EnemyType := GlobalTypes.EnemyTypes

const assault_round_max = 3
const assault_round_opt_time_in_sec: int = 180
const assault_time_points_deduction_per_sec: int = 5 # for every 5 second that is above opt time, points deduction

var enemies_per_round_template = {
	LevelNumber.FIRST: {
		"template_1": [
			[EnemyType.BASIC, EnemyType.BASIC, EnemyType.BASIC],
			[EnemyType.BASIC, EnemyType.ADVANCED, EnemyType.BASIC],
			[EnemyType.ADVANCED, EnemyType.ADVANCED, EnemyType.BASIC]
		],
		"template_test": [
			[EnemyType.BASIC],
			[EnemyType.BASIC],
			[EnemyType.ADVANCED]
		]
	}
}
