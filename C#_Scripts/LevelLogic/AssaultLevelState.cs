using Godot;
using System;
using System.Linq;

public partial class AssaultLevelState : LevelState
{
    private int currentAssaultRound = 0;
    private GlobalTypes.EnemyTypes[][] assaultLookup;
    private int assaultLookupIndex = 0;

    public override void _Ready()
    {
        enemy_manager.AllEnemiesDefeated += OnAssaultRoundFinished;
        base._Ready();
    }

    public override void Init()
    {
        level_number = RunState.level_number;
        string template = "template_1"; // TODO: make logic that gets template

        if (!AssaultGlobals.enemiesPerRoundTemplate.ContainsKey(level_number) ||
            !AssaultGlobals.enemiesPerRoundTemplate[level_number].ContainsKey(template))
        {
            GD.PushError($"Error. No assault spawn template found for level_number {level_number} and template string {template}");
            return;
        }

        assaultLookup = AssaultGlobals.enemiesPerRoundTemplate[level_number][template];

        if (assaultLookup == null || assaultLookup.Length == 0)
        {
            GD.PushError($"Error. No assault spawn template found for level_number {level_number} and template string {template}");
            return;
        }

        StartAssaultRound();
    }

    public void SelectTemplate()
    {
        // TODO
    }

    private async void OnAssaultRoundFinished()
    {
        currentAssaultRound++;

        if (currentAssaultRound >= AssaultGlobals.ASSAULT_ROUND_MAX)
        {
            LevelIsCompleted();
            return;
        }

        await PauseGame();
        EmitSignal(SignalName.StartRound, GetAssaultLookupRoundValue());
    }

    private Godot.Collections.Array<GlobalTypes.EnemyTypes> GetAssaultLookupRoundValue()
    {
        if (assaultLookup == null || assaultLookup.Length == 0)
        {
            GD.PushError("Can't increment assault_lookup in LevelState because it's null or empty");
            return [];
        }

        if (assaultLookupIndex >= assaultLookup.Length)
        {
            GD.PushError("Can't increment assault_lookup in LevelState because index out of range");
            return [];
        }

        var assaultTemplate = assaultLookup[assaultLookupIndex];
        assaultLookupIndex++;
        return ConvertToGodotArray(assaultTemplate);
    }

    private static Godot.Collections.Array<GlobalTypes.EnemyTypes> ConvertToGodotArray(GlobalTypes.EnemyTypes[] enemyKillResults)
	{
		var godotArray = new Godot.Collections.Array<GlobalTypes.EnemyTypes>();
		foreach (var enemyKillResult in enemyKillResults)
		{
			godotArray.Add(enemyKillResult);
		}

		return godotArray;
	}

    private async void StartAssaultRound()
    {
        await PauseGame();
        EmitSignal(SignalName.StartRound, GetAssaultLookupRoundValue());
        start_time = (int)(Time.GetTicksMsec() / 1000f);
    }
}

