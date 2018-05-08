using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolFollowCollision : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider collider)//当玩家进入范围内时，追踪玩家
    {
        if (collider.gameObject.tag == "Player")
        {
            gameObject.GetComponent<BoxCollider>().size = new Vector3(15, 1, 15);//将碰撞盒变大防止反复进入和退出范围
            Singleton<GameEventManager>.Instance.PlayerClose(gameObject);
        }
    }
    private void OnTriggerExit(Collider collider)//当玩家脱离范围时，停止追踪
    {
        if (collider.gameObject.tag == "Player")
        {
            Singleton<GameEventManager>.Instance.Escape(gameObject);
            gameObject.GetComponent<BoxCollider>().size = new Vector3(10, 1, 10);//恢复碰撞盒大小

        }
    }
}
