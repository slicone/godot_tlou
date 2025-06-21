using Godot;
using System;

public partial class StateMachine : Node
{
	[Export]
	public State StartingState { get; set; }

	private State _currentState;

	public void Init(Player parent)
	{
		foreach (var child in GetChildren())
		{
			if (child is State state)
			{
				state.Parent = parent;
			}
		}

		ChangeState(StartingState);
	}

	public void ChangeState(State newState)
	{
		if (_currentState != null)
		{
			_currentState.Exit();
		}

		_currentState = newState;
		_currentState.Enter();
	}

	public void ProcessPhysics(double delta)
	{
		var newState = _currentState?.ProcessPhysics(delta);
		if (newState != null)
		{
			ChangeState(newState);
		}
	}

	public void ProcessInput(InputEvent inputEvent)
	{
		var newState = _currentState?.ProcessInput(inputEvent);
		if (newState != null)
		{
			ChangeState(newState);
		}
	}

	public void ProcessFrame(double delta)
	{
		var newState = _currentState?.ProcessFrame(delta);
		if (newState != null)
		{
			ChangeState(newState);
		}
	}
}
