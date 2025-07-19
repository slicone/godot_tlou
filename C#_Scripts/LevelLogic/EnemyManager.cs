using Godot;
using System;

public partial class EnemyManager : Node
{
    [Export]
    public LevelState LevelState { get; set; }

    private PackedScene enemyScene = GD.Load<PackedScene>("res://Scenes/Enemies/enemy_base.tscn");

    [Signal]
    public delegate void AllEnemiesDefeatedEventHandler();

    [Signal]
    public delegate void EnemyDiedEventHandler(EnemyBase enemy, Godot.Collections.Array killStats);

    public override void _Ready()
    {
        LevelState.Connect("start_round", new Callable(this, nameof(SpawnEnemies)));
        LevelState.StartRound += SpawnEnemies;
    }

    public void RemoveEnemy(EnemyBase enemy)
    {
        RemoveChild(enemy);
    }

    private void SpawnEnemies(Godot.Collections.Array<GlobalTypes.EnemyTypes> numberOfEnemies)
    {
        for (int i = 0; i < numberOfEnemies.Count; i++) // TODO: spawn enemies dependent on type
        {
            EnemyBase enemyInstance = enemyScene.Instantiate<EnemyBase>();
            AddChild(enemyInstance);

            enemyInstance.EnemyDied += OnEnemyDied;
            enemyInstance.GlobalPosition = new Vector2(200 + i, 100 + i);
        }
    }

    public void SpawnEnemyDependentOnType(GlobalTypes.EnemyTypes enemyType)
    {
        // TODO
    }

    private void OnEnemyDied(EnemyBase enemy, Godot.Collections.Array<GlobalTypes.EnemyKillResult> killStats)
    {
        RemoveEnemy(enemy);
        EmitSignal(SignalName.EnemyDied, enemy, killStats);

        if (GetChildCount() == 0)
        {
            EmitSignal(SignalName.AllEnemiesDefeated);
        }
    }
}
