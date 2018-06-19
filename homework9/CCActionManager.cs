using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCActionManager : SSActionManager,ISSActionCallback {

    public FirstController SceneController;
    public SSAction action;

	// Use this for initialization
	protected void Start () {
        SceneController = (FirstController)SSDirector.GetInstance().CurrentSceneController;
        foreach (GameObject i in SceneController.tanks)
        {
            action = TankIdealAction.GetSSAction();
            this.RunAction(i, action, this);
        }
    }

    // Update is called once per frame
    protected new void Update () {               
        base.Update();
    }
    public void SSActionEvent(SSAction source, int intParam = 0, GameObject objectParam = null)
    {
        if (intParam == 0)
        {
            action = TankFollowAction.GetSSAction();
            this.RunAction(objectParam, action, this);

        }
        else if (intParam == 1)
        {
            action = TankIdealAction.GetSSAction();
            this.RunAction(objectParam, action, this);
        }
    }
}
