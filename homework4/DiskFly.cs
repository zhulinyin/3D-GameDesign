using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskFly : SSAction {

    private float speed;
    private Vector3 direction;
    public static DiskFly GetSSAction()
    {
        DiskFly action = ScriptableObject.CreateInstance<DiskFly>();
        return action;
    }
    // Use this for initialization
    public override void Start()
    {
        speed = gameobject.GetComponent<DiskData>().speed;
        direction = gameobject.GetComponent<DiskData>().direction;
    }

    // Update is called once per frame
    public override void Update () {
        transform.Translate(direction * speed * Time.deltaTime);
        this.destroy = true;
        this.callback.SSActionEvent(this);
    }
}
