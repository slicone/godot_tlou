class_name AssaultScoreSystem
extends ScoreSystem
	
func calculate_score():
	is_meele_only()
	var dynamic_score = collect_points_from_dynamic_default_stats()
	var dynamics_score_mode = collect_points_from_dynamics_mode_stats()
	var score_static =  collect_points_from_static_stats(encounter_stats_static, points_lookup_table_static)
	current_score = score_static + dynamic_score + dynamics_score_mode
	print(current_score)

# calculate score for dynamic mode specific stats
func collect_points_from_dynamics_mode_stats():
	if not round_time:
		push_error("round_time not set in ScoreSystem")
	encounter_stats_dynamic["performance"]["time_completed"] = round_time
	return calculate_timer_score(round_time)

func calculate_timer_score(round_time: int):
	var round_time_points: int = points_lookup_table_dynamic["performance"]["time_completed"]
	if round_time > AssaultGlobals.assault_round_opt_time_in_sec:
		var exceeded_time: int = round_time - AssaultGlobals.assault_round_opt_time_in_sec
		# reduce 1 point for every points_decution_per_sec
		var points_deduction: int = exceeded_time / AssaultGlobals.assault_time_points_deduction_per_sec
		round_time_points = round_time_points - points_deduction
		if round_time_points < 0:
			round_time_points = 0
	return round_time_points

# TODO basis klasse?
func _calculate_accuracy():
	pass
	
# TODO basis klasse?
func _player_damage_taken():
	pass
