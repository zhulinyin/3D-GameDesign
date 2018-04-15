using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { NOT_RUN,ROUND_START,ROUND_OVER,GAMEOVER};
public interface IUserAction {

    void GameOver();
    void SetGameState(GameState gameState);
    GameState GetGameState();
    void Hit(Vector3 pos);
    void Restart();
    int GetRound();
}
