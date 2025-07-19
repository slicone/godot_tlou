using Godot;
using System;
using System.Collections.Generic;

public partial class AssaultScoreSystem : ScoreSystem
{
    public override void CalculateScore()
    {
        IsMeeleOnly();

        int dynamicScore = CollectPointsFromDynamicDefaultStats();
        int dynamicModeScore = CollectPointsFromDynamicsModeStats();
        int staticScore = CollectPointsFromStaticStats(encounter_stats_static, points_lookup_table_static);

        current_score = staticScore + dynamicScore + dynamicModeScore;
    }

    // Calculate score for dynamic mode-specific stats
    public override int CollectPointsFromDynamicsModeStats()
    {
        if (round_time == 0)
        {
            GD.PushError("round_time not set in ScoreSystem");
        }

        ((Dictionary<string, object>)encounter_stats_dynamic["performance"])["time_completed"] = round_time;
        return CalculateTimerScore(round_time);
    }

    private int CalculateTimerScore(int roundTime)
    {
        int roundTimePoints = (int) ((Dictionary<string, object>)points_lookup_table_dynamic["performance"])["time_completed"];

        if (roundTime > AssaultGlobals.ASSAULT_ROUND_OPT_TIME_IN_SEC)
        {
            int exceededTime = roundTime - AssaultGlobals.ASSAULT_ROUND_OPT_TIME_IN_SEC;

            // Reduce 1 point for every interval exceeded
            int pointsDeduction = exceededTime / AssaultGlobals.ASSAULT_TIME_POINTS_DEDUCTION_PER_SEC;
            roundTimePoints -= pointsDeduction;

            if (roundTimePoints < 0)
                roundTimePoints = 0;
        }

        return roundTimePoints;
    }

    // TODO: move to base ScoreSystem class if shared
    protected virtual void CalculateAccuracy()
    {
        // TODO: implement or override in subclass
    }

    // TODO: move to base ScoreSystem class if shared
    protected virtual void PlayerDamageTaken()
    {
        // TODO: implement or override in subclass
    }
}
