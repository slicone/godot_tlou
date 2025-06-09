extends Node

var LevelNumber := GlobalTypes.LevelNumber
var EnemyType := GlobalTypes.EnemyTypes

const assault_round_max = 3
const preperation_time: float = 2

var enemies_per_round_template = {
	LevelNumber.FIRST: {
		"template_1": [
			[EnemyType.BASIC, EnemyType.BASIC, EnemyType.BASIC],
			[EnemyType.BASIC, EnemyType.ADVANCED, EnemyType.BASIC],
			[EnemyType.ADVANCED, EnemyType.ADVANCED, EnemyType.BASIC]
		]
	}
}
