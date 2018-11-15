using UnityEngine;
using System.Collections;

public class GameStateManager : Singleton<GameStateManager> {

	private StateMachine gameState = new StateMachine();

	public string currentStateName
	{
		get
		{
			IState currentState = this.GetCurrentState();
            return currentState.name;

        }
	}

	public IState GetCurrentState()
	{
		return this.gameState.TopState();
	}

	public void Initialize()
	{
		this.gameState.RegisterStateByAttributes<GameStateAttribute>(typeof(GameStateAttribute).Assembly);
	}

	public void Uninitialize()
	{
		this.gameState.Clear();
		this.gameState = null;
	}

	public void GotoState(string name)
	{
        AudioManager.Instance.StopSoundAndBackgroundSoundsOnSceneChange();
        this.gameState.ChangeState(name);
    }
}
