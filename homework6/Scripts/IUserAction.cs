using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { NOT_RUN,ROUND_START,ROUND_OVER,GAMEOVER};
public interface IUserAction {

    void GameOver();
    void SetGameState(GameState gameState);
    GameState GetGameState();
    void Restart();
    void PlayerMove(Vector3 target);
    void PlayerRotate(Vector3 direction);
    void SpeedUp();
}
