using Godot;
using System;
using System.Collections.Generic;

public partial class LevelManager : Node
{
    public static LevelManager Instance { get; private set; }

    public override void _Ready()
    {
        Instance = this;
    }


    private List<string> levelScenes = new()
    {
        "res://levels/Assault_1.tscn"
    };

    private GlobalTypes.LevelNumber[] levelNumbers = new GlobalTypes.LevelNumber[]
    {
        GlobalTypes.LevelNumber.FIRST,
        GlobalTypes.LevelNumber.SECOND,
        GlobalTypes.LevelNumber.THIRD,
        GlobalTypes.LevelNumber.FOURTH,
        GlobalTypes.LevelNumber.FIFTH,
        GlobalTypes.LevelNumber.SIXTH
    };

    private int levelIndex = 0;
    private string lastLevelPath = "";

    public Node2D InstantiateLevel(string levelPath, GlobalTypes.LevelNumber levelNumber)
    {
        var levelScene = GD.Load<PackedScene>(levelPath);
        var levelInstance = levelScene.Instantiate() as Node2D;

        if (levelInstance == null)
        {
            GD.PushError($"Failed to instantiate level from path: {levelPath}");
        }

        return levelInstance;
    }

    public void IncrementLevelNumber()
    {
        if (levelIndex >= levelNumbers.Length)
        {
            GD.PushWarning("No more levels to increment to.");
            return;
        }

        RunState.level_number = levelNumbers[levelIndex];
        levelIndex++;
    }

    public void ChangeLevel(string levelPath, GlobalTypes.LevelNumber levelNumber)
    {
        var levelInstance = InstantiateLevel(levelPath, levelNumber);
        if (levelInstance == null)
            return;

        // Remove old scene
        GetTree().CurrentScene?.QueueFree();

        // Add and set new scene
        GetTree().Root.AddChild(levelInstance);
        GetTree().CurrentScene = levelInstance;

        IncrementLevelNumber();

        // Optional: Call Init() on LevelState
        // CallLevelStateInit(levelInstance, levelPath, levelNumber);
    }

    public string GetRandomLevelPath(List<string> availableLevels)
    {
        if (availableLevels.Count == 0)
        {
            GD.PushError("No available levels to choose from.");
            return "";
        }

        var randIndex = GD.Randi() % (uint)availableLevels.Count;
        var nextLevel = availableLevels[(int)randIndex];
        lastLevelPath = nextLevel;
        return nextLevel;
    }

    public void LoadRandomLevel()
    {
        levelScenes.Remove(lastLevelPath);

        if (levelScenes.Count == 0)
        {
            GD.PushError("No levels left to load!");
            return;
        }

        string nextLevel = GetRandomLevelPath(levelScenes);
        ChangeLevel(nextLevel, GlobalTypes.LevelNumber.FIRST); // TODO: use actual level number?
    }

    // Optional: Call Init() on level state if needed
    private void CallLevelStateInit(Node2D levelInstance, string levelPath, GlobalTypes.LevelNumber levelNumber)
    {
        foreach (Node child in GetChildren())
        {
            if (child is LevelState state)
            {
                state.Init(); // or pass params if needed
                return;
            }
        }

        GD.PushError($"Level with path {levelPath} doesn't have a LevelState child.");
    }

    // Placeholder for roadmap logic
    public void GenerateLevelRoadMap()
    {
        // TODO: implement
    }
}
