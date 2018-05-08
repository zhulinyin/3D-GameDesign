using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionController : MonoBehaviour {
    private Dictionary<int, Vector3> directions;//存储东南西北四个方向，巡逻兵沿矩形路径巡逻
    // Use this for initialization
    void Awake () {
        directions = new Dictionary<int, Vector3>();
        directions.Add(0, new Vector3(1, 0, 0));
        directions.Add(1, new Vector3(0, 0, 1));
        directions.Add(2, new Vector3(-1, 0, 0));
        directions.Add(3, new Vector3(0, 0, -1));
    }
	
	public Vector3 RandomDirection()//随机获得一个方向
    {
        return directions[Random.Range(0, 4)];
    }

    public Vector3 ChangeDirection(Vector3 direction)//改变方向
    {
        if (direction.Equals(directions[0]))
        {
            return directions[1];
        }
        else if (direction.Equals(directions[1]))
        {
            return directions[2];
        }
        else if (direction.Equals(directions[2]))
        {
            return directions[3];
        }
        else if (direction.Equals(directions[3]))
        {
            return directions[0];
        }
        return RandomDirection();
    }
}
