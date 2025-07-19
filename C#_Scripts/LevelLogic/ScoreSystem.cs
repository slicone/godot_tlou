using Godot;
using System;
using System.Collections.Generic;

public partial class ScoreSystem : Node
{
    [Export]
    public EnemyManager enemy_manager;

    protected int current_score = 0;
    protected int round_time;

    // stats that can be just calculated with the corresponding point dict
    protected Dictionary<string, object> encounter_stats_static = new()
    {
        {
            "performance", new Dictionary<string, object>()
            {
                { "encounter_completion", false }
            }
        },
        {
            "bonus", new Dictionary<string, object>()
            {
                { "gambit_complete", false },
                { "stuns", 0 },
                { "stealth_kills", 0 },
                { "melee_kills", 0 },
                { "headshot_kills", 0 },
                { "headshot_only", false },
                { "melee_only", false },
                { "unseen", false }
            }
        },
        {
            "tracking_only", new Dictionary<string, object>()
            {
                { "gun_kills", 0 },
                { "gadget_kills", 0 }
            }
        }
    };


    // Dynamic stats need to be caluclated specially, because points vary on player performance
    protected Dictionary<string, object> encounter_stats_dynamic = new()
    {
        {
            "performance", new Dictionary<string, object>()
            {
                { "damage_taken", 0 },
                { "accuracy", 0 },
                { "time_completed", 0 },
            }
        }
    };

    // points will not change
    protected readonly Dictionary<string, object> points_lookup_table_static = new()
    {
        {
            "performance", new Dictionary<string, int> ()
            {
                { "encounter_completion", 100 },
                { "enemies_killed", 10 }
            }
        },
        {
            "bonus", new Dictionary<string, int>()
            {
                { "gambit_complete", 10 },
                { "stuns", 5 },
                { "stealth_kills", 5 },
                { "melee_kills", 5 },
                { "headshot_only", 50 },
                { "melee_only", 50 },
                { "unseen", 50 },
                { "safe_captured", 50 }
            }
        }
    };


    // Max values of dynamic points
    protected readonly Dictionary<string, object> points_lookup_table_dynamic = new()
    {
        {
            "performance", new Dictionary<string, int>()
            {
                { "damage_taken", 100 },
                { "accuracy", 100 },
                { "time_completed", 100 },
                { "ally_damage_taken", 100 }
            }
        }
    };

    public override void _Ready()
    {
        enemy_manager.Connect("enemy_died", new Callable(this, nameof(AddKillResult)));
    }

    // TODO why enemy as parameter?
    private void AddKillResult(EnemyBase enemy, GlobalTypes.EnemyKillResult[] killResult)
    {
        foreach (var result in killResult)
        {
            var encounterLogBonus = (Dictionary<string, object>) encounter_stats_static["bonus"];
            var encounterTrackOnly = (Dictionary<string, object>) encounter_stats_static["tracking_only"];

            switch (result)
            {
                case GlobalTypes.EnemyKillResult.HEADSHOT:
                    encounterLogBonus["headshot_kills"] = (int)encounterLogBonus["headshot_kills"] + 1;
                    break;
                case GlobalTypes.EnemyKillResult.MELEE:
                    encounterLogBonus["melee_kills"] = (int)encounterLogBonus["melee_kills"] + 1;
                    break;
                case GlobalTypes.EnemyKillResult.STUNNED:
                    encounterLogBonus["stuns"] = (int)encounterLogBonus["stuns"] + 1;
                    break;
                case GlobalTypes.EnemyKillResult.STEALTH:
                    encounterLogBonus["stealth_kills"] = (int)encounterLogBonus["stealth_kills"] + 1;
                    break;
                case GlobalTypes.EnemyKillResult.GUN:
                    encounterTrackOnly["gun_kills"] = (int)encounterTrackOnly["gun_kills"] + 1;
                    break;
                default:
                    GD.PushError("Unknown kill result");
                    break;
            }
        }
    }

    // Default score logic for stats shared across modes
    public virtual int CollectPointsFromDynamicDefaultStats()
    {
        // TODO: implement damage and accuracy score
        return 0;
    }

    public int CollectPointsFromStaticStats(Dictionary<string, object> stats, Dictionary<string, object> points, List<string> keyPath = null, int total = 0)

    {
        keyPath ??= [];

        foreach (var key in stats.Keys)
        {
            var value = stats[key];
            //var newPath = keyPath;
            keyPath.Add(key);

            if (value is Dictionary<string, object> nested)
            {
                total = CollectPointsFromStaticStats(nested, points, keyPath, total);
            }
            else
            {
                if (value is bool b && !b || value is int i && i == 0)
                    continue;

                int count = (value is bool && (bool)value) ? 1 : Convert.ToInt32(value);
                var pointValue = GetValueAtPath(points, keyPath, count);

                if (pointValue is int intValue)
                    total += intValue;
                else if (pointValue is float floatValue)
                    total += (int)floatValue;
            }
        }

        return total;
    }

    public object GetValueAtPath(Dictionary<string, object> dictValue, List<string> path, int count)
    {
        object current = dictValue;

        foreach (var key in path)
        {
            if (current is Dictionary<string, object> currentDict && currentDict.ContainsKey(key))
            {
                current = currentDict[key];
            }
            else
            {
                GD.Print("ScoreSystem: Key path from stats dict not found in point dict");
                return null;
            }
        }

        return current is int i ? i * count :
               current is float f ? f * count : null;
    }

    public void SetRoundTimer(int roundTime)
    {
        this.round_time = roundTime;
    }

    public void SetEncounterValue(bool value)
    {
        var perf = (Dictionary<string, object>) encounter_stats_static["performance"];
        perf["encounter_completion"] = value;
    }

    public int GetScore()
    {
        return current_score;
    }

    public object[] GetEncounterStats()
    {
        return [encounter_stats_static, encounter_stats_dynamic];
    }

    public void IsMeeleOnly()
    {
        var bonus = (Dictionary<string, object>) encounter_stats_static["bonus"];
        var tracking = (Dictionary<string, object>) encounter_stats_static["tracking_only"];
        bonus["melee_only"] = ((int)tracking["gun_kills"]) == 0;
    }

    public void SetPlayerDamage(int damage)
    {
        // TODO: store in dynamic stats
    }

    public virtual void IsHeadshotOnly()
    {
        // TODO: implement
    }

    public virtual void CalculateScore()
    {
        GD.PushError("Needs to be implemented by child class");
    }

    public virtual int CollectPointsFromDynamicsModeStats()
    {
        GD.PushError("Needs to be implemented by child class");
        return 0;
    }
}
