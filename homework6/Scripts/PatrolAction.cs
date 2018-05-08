using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolAction : SSAction {

    private float speed = 5;
    private Vector3 direction;
    private float distance;
    private float turnSpeed = 10;
    public static PatrolAction GetSSAction()
    {
        PatrolAction action = ScriptableObject.CreateInstance<PatrolAction>();
        return action;
    }
    // Use this for initialization
    public override void Start()
    {
        direction = gameobject.GetComponent<PatrolData>().direction;
        distance = gameobject.GetComponent<PatrolData>().distance;        
    }

    // Update is called once per frame
    public override void Update () {
        if (gameobject.GetComponent<PatrolData>().follow)
        {
            this.destroy = true;
            this.callback.SSActionEvent(this,0,gameobject);
            return;
        }
        direction = gameobject.GetComponent<PatrolData>().direction;
        distance = gameobject.GetComponent<PatrolData>().distance;
        if (distance < 10)//巡逻兵移动
        {
            transform.position += direction * speed * Time.deltaTime;
            //将方向转换为四元数  
            Quaternion quaDir = Quaternion.LookRotation(direction, Vector3.up);
            //缓慢转动到目标点  
            transform.rotation = Quaternion.Lerp(transform.rotation, quaDir, Time.fixedDeltaTime * turnSpeed);
            gameobject.GetComponent<PatrolData>().distance += speed * Time.deltaTime;
            
        }
        else//巡逻兵转向
        {
            gameobject.GetComponent<PatrolData>().distance = 0;
            gameobject.GetComponent<PatrolData>().direction = 
                Singleton<DirectionController>.Instance.ChangeDirection(direction);
        }
        
    }
}
