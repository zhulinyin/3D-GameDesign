using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCActionManager : SSActionManager,ISSActionCallback {

    public FirstController SceneController;
    public DiskFly diskfly;

	// Use this for initialization
	protected void Start () {
        SceneController = (FirstController)SSDirector.GetInstance().CurrentSceneController;
	}

    // Update is called once per frame
    protected new void Update () {       
        
        foreach(GameObject i in SceneController.Disks)//所有飞碟执行动作
        {
            diskfly = DiskFly.GetSSAction();
            this.RunAction(i, diskfly, this);
        }
        base.Update();
    }
    public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Completed,
        int intParam = 0, string strParam = null, Object objectParam = null)
    {

    }
}
