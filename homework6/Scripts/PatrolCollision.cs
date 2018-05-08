using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolCollision : MonoBehaviour {
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")//当巡逻兵碰撞上玩家时，游戏结束
        {
            Singleton<GameEventManager>.Instance.PlayerCollide();
            gameObject.GetComponentInChildren<Animator>().SetTrigger("attack");
        }
        else//当巡逻兵碰撞到其他物体如墙壁时，转换方向
        {
            gameObject.GetComponent<PatrolData>().distance = 0;
            gameObject.GetComponent<PatrolData>().direction = 
                Singleton<DirectionController>.Instance.ChangeDirection(gameObject.GetComponent<PatrolData>().direction);
        }
    }    
}
