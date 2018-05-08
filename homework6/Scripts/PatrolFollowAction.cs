using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolFollowAction : SSAction
{
    private GameObject player;
    private float speed = 5.8f;
    public static PatrolFollowAction GetSSAction()
    {
        PatrolFollowAction action = ScriptableObject.CreateInstance<PatrolFollowAction>();
        return action;
    }
    // Use this for initialization
    public override void Start()
    {
        player = gameobject.GetComponent<PatrolData>().player;
    }

    // Update is called once per frame
    public override void Update()
    {
        if (!gameobject.GetComponent<PatrolData>().follow)
        {
            this.destroy = true;
            this.callback.SSActionEvent(this,1,gameobject);
            return;
        }
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        transform.LookAt(player.transform.position);
    }
}
