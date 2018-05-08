using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventManager : MonoBehaviour {

    public delegate void GameOver();
    public static event GameOver OnGameOver;

    public delegate void PatrolFollow(GameObject ob);
    public static event PatrolFollow OnPatrolFollow;

    public delegate void PlayerEscape(GameObject ob);
    public static event PlayerEscape OnPlayerEscape;

    public void PlayerCollide()//玩家碰撞
    {
        if (OnGameOver != null)
        {
            OnGameOver();
        }
    }

    public void PlayerClose(GameObject ob)//玩家靠近巡逻兵
    {
        if (OnPatrolFollow != null)
        {
            OnPatrolFollow(ob);
        }
    }

    public void Escape(GameObject ob)//玩家脱离巡逻兵
    {
        if (OnPlayerEscape != null)
        {
            OnPlayerEscape(ob);
        }
    }
}
