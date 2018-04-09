using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetOnBoat : SSAction {

    public FirstController SceneController;
    public static GetOnBoat GetSSAction()
    {
        GetOnBoat action = ScriptableObject.CreateInstance<GetOnBoat>();
        return action;
    }
	// Use this for initialization
	public override void Start () {
        SceneController = (FirstController)SSDirector.GetInstance().CurrentSceneController;
	}

    // Update is called once per frame
    public override void Update () {
        if (((SceneController.boatPosition == FirstController.BoatPosition.left &&
            SceneController.Left.Contains(SceneController.PlayerMove)) ||
            (SceneController.boatPosition == FirstController.BoatPosition.right &&
            SceneController.Right.Contains(SceneController.PlayerMove)))
                && SceneController.OnTheBoat.Count < 2)//判断船和被点击人物是否在同一岸边且船未坐满
        {
            if (SceneController.pos1 == null)//如果船的第一个位置不为空则把人物移动到第一个位置
            {
                SceneController.PlayerMove.transform.position = SceneController.boat.transform.position +
                    new Vector3(-0.5f, 1, 0);
                SceneController.pos1 = SceneController.PlayerMove;
            }
            else//否则把人物移动到第二个位置
            {
                SceneController.PlayerMove.transform.position = SceneController.boat.transform.position +
                    new Vector3(0.5f, 1, 0);
                SceneController.pos2 = SceneController.PlayerMove;
            }
            SceneController.PlayerMove.transform.parent = SceneController.boat.transform;//将船设为人物的父对象
            if (SceneController.boatPosition == FirstController.BoatPosition.left)//从对应的岸中移除人物
            {
                SceneController.Left.Remove(SceneController.PlayerMove);
            }
            else if (SceneController.boatPosition == FirstController.BoatPosition.right)
            {
                SceneController.Right.Remove(SceneController.PlayerMove);
            }
            SceneController.OnTheBoat.Add(SceneController.PlayerMove);//将人物添加到船上
        }
        SceneController.PlayerMove = null;//将点击的人物清空
        this.destroy = true;
        this.callback.SSActionEvent(this);
    }
}
