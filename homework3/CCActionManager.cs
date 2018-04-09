using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCActionManager : SSActionManager,ISSActionCallback {

    public FirstController SceneController;
    public GetOffBoat GetOff;
    public GetOnBoat GetOn;
    public BoatMoving Moving;

	// Use this for initialization
	protected void Start () {
        SceneController = (FirstController)SSDirector.GetInstance().CurrentSceneController;
	}

    // Update is called once per frame
    protected new void Update () {
        if (SceneController.state == FirstController.GameState.fail || 
            SceneController.state == FirstController.GameState.win) return;//如果游戏胜利或者失败就不再更新

        if (Input.GetMouseButtonDown(0) && !SceneController.BoatMove)//如果船不在移动，判断是否点击鼠标左键
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//从相机处发出射线
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject.tag == "devil" || hit.collider.gameObject.tag == "priest")//判断射线击中点是否是恶魔或者牧师
                {
                    SceneController.PlayerMove = hit.collider.gameObject;//记录下被点击的人物
                }
            }
        }
        if (SceneController.PlayerMove != null && SceneController.PlayerMove.transform.parent == null)//被点击人物不为空且其父对象为空，说明被点击人物在岸上
        {
            GetOn = GetOnBoat.GetSSAction();
            this.RunAction(SceneController.PlayerMove, GetOn, this);
        }
        if (SceneController.PlayerMove != null && SceneController.PlayerMove.transform.parent != null)//被点击的人物不为空且其父对象不为空，说明人物在船上
        {
            GetOff = GetOffBoat.GetSSAction();
            this.RunAction(SceneController.PlayerMove, GetOff, this);
        }
        if (SceneController.BoatMove)//判断船是否在移动
        {
            Moving = BoatMoving.GetSSAction();
            this.RunAction(SceneController.boat, Moving, this);
        }
        base.Update();
    }
    public void SSActionEvent(SSAction source, SSActionEventType events = SSActionEventType.Completed,
        int intParam = 0, string strParam = null, Object objectParam = null)
    {

    }
}
