using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCActionManager : SSActionManager,ISSActionCallback {

    public FirstController SceneController;
    public SSAction action;

	// Use this for initialization
	protected void Start () {
        SceneController = (FirstController)SSDirector.GetInstance().CurrentSceneController;
        foreach (GameObject i in SceneController.Patrols)
        {
            action = PatrolAction.GetSSAction();
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
            action = PatrolFollowAction.GetSSAction();
        }
        else if (intParam == 1)
        {
            action = PatrolAction.GetSSAction();
        }
        this.RunAction(objectParam, action, this);
    }
}
