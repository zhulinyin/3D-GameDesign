using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    Vector3 dir;
    public GameObject player;
	// Use this for initialization
	void Start () {
        player = SSDirector.GetInstance().CurrentSceneController.GetPlayer();
        dir = player.transform.position - transform.position;
	}

    private void LateUpdate()//实现相机跟随玩家
    {
        transform.position = player.transform.position - dir;
    }
}
