using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMoving : SSAction {

    public FirstController SceneController;
    public static BoatMoving GetSSAction()
    {
        BoatMoving action = ScriptableObject.CreateInstance<BoatMoving>();
        return action;
    }
    // Use this for initialization
    public override void Start()
    {
        SceneController = (FirstController)SSDirector.GetInstance().CurrentSceneController;
    }

    // Update is called once per frame
    public override void Update () {
        if (SceneController.boatPosition == FirstController.BoatPosition.right)//如果船在右边则向左移动
        {
            SceneController.CheckState1(SceneController.Right);
            SceneController.boat.transform.Translate(Vector3.left * Time.deltaTime * 3);
            if (SceneController.boat.transform.position.x <= -4)
            {
                SceneController.BoatMove = false;
                SceneController.boatPosition = FirstController.BoatPosition.left;
                SceneController.CheckState2(SceneController.Left);
            }
        }
        if (SceneController.boatPosition == FirstController.BoatPosition.left)//如果船在左边则向右移动
        {
            SceneController.CheckState1(SceneController.Left);
            SceneController.boat.transform.Translate(Vector3.right * Time.deltaTime * 3);
            if (SceneController.boat.transform.position.x >= 4)
            {
                SceneController.BoatMove = false;
                SceneController.boatPosition = FirstController.BoatPosition.right;
                SceneController.CheckState2(SceneController.Right);
            }
        }
        this.destroy = true;
        this.callback.SSActionEvent(this);
    }
}
