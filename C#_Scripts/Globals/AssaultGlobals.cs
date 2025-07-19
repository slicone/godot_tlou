using Godot;
using System.Collections.Generic;

using EnemyTypes = GlobalTypes.EnemyTypes;
using LevelNumber = GlobalTypes.LevelNumber;

public partial class AssaultGlobals : Node
{

    public const int ASSAULT_ROUND_MAX = 3;
    public const int ASSAULT_ROUND_OPT_TIME_IN_SEC = 180;
    public const int ASSAULT_TIME_POINTS_DEDUCTION_PER_SEC = 5;

    // enemiesPerRoundTemplate[GlobalTypes.LevelNumber][template_name][round] => List<GlobalTypes.EnemyTypes>
    public static readonly Dictionary<LevelNumber, Dictionary<string, EnemyTypes[][]>> enemiesPerRoundTemplate = new()
    {
        {
            LevelNumber.FIRST, new Dictionary<string, EnemyTypes[][]>
            {
                {
                    "template_1", new EnemyTypes[][]
                    {
                        [EnemyTypes.BASIC, EnemyTypes.BASIC, EnemyTypes.BASIC],
                        [EnemyTypes.BASIC, EnemyTypes.ADVANCED, EnemyTypes.BASIC],
                        [EnemyTypes.ADVANCED, EnemyTypes.ADVANCED, EnemyTypes.BASIC]
                    }
                },
                {
                    "template_test", new EnemyTypes[][]
                    {
                        [EnemyTypes.BASIC],
                        [EnemyTypes.BASIC],
                        [EnemyTypes.ADVANCED]
                    }
                }
            }
        }
    };
}
