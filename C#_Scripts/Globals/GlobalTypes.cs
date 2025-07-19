using Godot;

public partial class GlobalTypes : Node
{
	public enum EnemyKillResult
	{
		HEADSHOT,
		MELEE,
		GUN,
		STUNNED,
		STEALTH,
		NONE
	}

	public enum LevelNumber
	{
		FIRST,
		SECOND,
		THIRD,
		FOURTH,
		FIFTH,
		SIXTH
	}

	public enum EnemyTypes
	{
		BASIC,
		ADVANCED,
		ELITE,
		BOSS
	}

	

	public enum ResourceType
	{
		ETHANOL,
		PAPER,
		PLASTIC,
		GUNPOWDER,
		TAPE,
		SCISSOR
	}

	public enum PlayerAnimationState
	{
		NOGUN,
		RANGEWEAPON,
		MELEEWEAPON
	}

	public const float PreparationTimeInSec = 2f;


}