using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankFollowCollisioin : MonoBehaviour {

    private void OnTriggerEnter(Collider collider)  //当玩家进入范围内时，追踪玩家，并放大碰撞盒，防止频繁进出碰撞盒
    {
        if (collider.gameObject.tag == "Player")
        {
            gameObject.GetComponent<BoxCollider>().size = new Vector3(60, 1, 60);
            GetComponent<TankData>().follow = true;
        }
    }
    private void OnTriggerExit(Collider collider)   //当玩家脱离范围时，停止追踪，并恢复碰撞盒大小
    {
        if (collider.gameObject.tag == "Player")
        {
            gameObject.GetComponent<BoxCollider>().size = new Vector3(50, 1, 50);
            GetComponent<TankData>().follow = false;
        }
    }
}
