using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskFly : SSAction {

    private float speed;
    private Vector3 direction;
    private Rigidbody rigidbody;
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
        rigidbody = gameobject.GetComponent<Rigidbody>();
        if (rigidbody)
        {
            rigidbody.velocity = direction * speed;
        }
    }

    // Update is called once per frame
    public override void Update () {
        if (gameobject.activeSelf)
        {
            transform.Translate(direction * speed * Time.deltaTime);
            if (transform.position.x > 20 || transform.position.y > 12 || transform.position.y < -12)
            {
                this.destroy = true;
                this.enable = false;
                this.callback.SSActionEvent(this);
            }
        }
        else
        {
            this.destroy = true;
            this.enable = false;
            this.callback.SSActionEvent(this);
        }
    }

    public override void FixedUpdate()
    {
        if (gameobject.activeSelf)
        {
            if (transform.position.x > 20 || transform.position.y > 12 || transform.position.y < -12)
            {
                this.destroy = true;
                this.enable = false;
                this.callback.SSActionEvent(this);
            }
        }
        else
        {
            this.destroy = true;
            this.enable = false;
            this.callback.SSActionEvent(this);
        }
    }
}
