using Godot;
using System.Text.Json;

public abstract partial class LevelState : Node
{
    [Signal]
    public delegate void LevelFailedEventHandler();

    [Signal]
    public delegate void StartRoundEventHandler(Godot.Collections.Array<GlobalTypes.EnemyTypes> enemiesToSpawn);

    [Signal]
    public delegate void LevelCompletedEventHandler();

    [Export]
    public EnemyManager enemy_manager;

    [Export]
    public ScoreSystem score_system;

    [Export]
    public Player player;

    protected int start_time = 0;
    protected int elapsed_time = 0;
    protected GlobalTypes.LevelNumber level_number;

    public override void _Ready()
    {
        player.PlayerDied += OnPlayerDied;
        Init();
    }

    public abstract void Init();

    public SignalAwaiter PauseGame()
    {
        return ToSignal(GetTree().CreateTimer(GlobalTypes.PreparationTimeInSec), "timeout");
    }

    private void OnEnemiesDefeated()
    {
        Win("All enemies defeated");
        LevelManager.Instance.LoadRandomLevel();
    }

    private void OnTimeOut()
    {
        Lose("Time ran out");
    }

    public void LevelIsCompleted()
    {
        elapsed_time = (int)(Time.GetTicksMsec() / 1000) - start_time;
        int round_time = elapsed_time - 3 * (int) GlobalTypes.PreparationTimeInSec;

        score_system.SetEncounterValue(true);
        score_system.SetRoundTimer(round_time);
        score_system.CalculateScore();
        GD.Print(score_system.GetScore());

        string encounterStatsStatic = JsonSerializer.Serialize(score_system.GetEncounterStats()[0]);
            string encounterStatsDynamic = JsonSerializer.Serialize(score_system.GetEncounterStats()[1]);

            GD.Print("static: \n" + encounterStatsStatic);
            GD.Print("dynamic: \n" + encounterStatsDynamic);

            LevelManager.Instance.LoadRandomLevel();
            GD.Print("level completed");
        }

        private void OnPlayerDied()
        {
            score_system.SetEncounterValue(false);
            score_system.CalculateScore();
            Lose("Player died");
        }

        private void Win(string reason)
        {
            GD.Print($"LEVEL COMPLETE: {reason}");
            EmitSignal(SignalName.LevelCompleted);
    }

    private void Lose(string reason)
    {
        GD.Print($"LEVEL FAILED: {reason}");
        EmitSignal(SignalName.LevelFailed);
    }
}

