using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetOffBoat : SSAction {

    public FirstController SceneController;
    public static GetOffBoat GetSSAction()
    {
        GetOffBoat action = ScriptableObject.CreateInstance<GetOffBoat>();
        return action;
    }
    // Use this for initialization
    public override void Start()
    {
        SceneController = (FirstController)SSDirector.GetInstance().CurrentSceneController;
    }

    // Update is called once per frame
    public override void Update () {
        if (SceneController.boatPosition == FirstController.BoatPosition.right)//判断船的位置
        {
            SceneController.PlayerMove.transform.position = new Vector3(5.5f + 
                Convert.ToInt32(SceneController.PlayerMove.name), 0, 0);
            SceneController.PlayerMove.transform.parent = null;
            SceneController.Right.Add(SceneController.PlayerMove);
            if (SceneController.Right.Count == 6) SceneController.state = 
                    FirstController.GameState.win;//如果右岸人数为6，则游戏胜利
        }
        else if (SceneController.boatPosition == FirstController.BoatPosition.left)
        {
            SceneController.PlayerMove.transform.position = new Vector3(-11 + 
                Convert.ToInt32(SceneController.PlayerMove.name), 0, 0);
            SceneController.PlayerMove.transform.parent = null;
            SceneController.Left.Add(SceneController.PlayerMove);
        }
        if (SceneController.pos1 == SceneController.PlayerMove) SceneController.pos1 = null;
        else SceneController.pos2 = null;
        SceneController.OnTheBoat.Remove(SceneController.PlayerMove);
        SceneController.PlayerMove = null;
        this.destroy = true;
        this.callback.SSActionEvent(this);
    }
}
