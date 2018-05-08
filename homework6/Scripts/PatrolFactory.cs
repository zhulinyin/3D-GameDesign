using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolFactory : MonoBehaviour {

    public GameObject patrolPrefab;
    
    public List<GameObject> GetPatrol()
    {
        GameObject newObject;
        List<GameObject> patrols=new List<GameObject>();
        int[] pos_x = { -15, 0, 15 };
        int[] pos_z = { -20, 20 };
        for(int i = 0; i < 3; i++)
        {
            for(int j = 0; j < 2; j++)
            {
                newObject = Instantiate<GameObject>(patrolPrefab, new Vector3(pos_x[i], 0, pos_z[j]), Quaternion.identity);
                newObject.GetComponent<PatrolData>().position = new Vector3(pos_x[i], 0, pos_z[j]);
                newObject.GetComponent<PatrolData>().follow = false;
                newObject.GetComponent<PatrolData>().player = SSDirector.GetInstance().CurrentSceneController.GetPlayer();
                newObject.GetComponent<PatrolData>().direction = Singleton<DirectionController>.Instance.RandomDirection();
                newObject.GetComponent<PatrolData>().distance = 0;
                patrols.Add(newObject);
            }
        }
        return patrols;
    }
    
}
