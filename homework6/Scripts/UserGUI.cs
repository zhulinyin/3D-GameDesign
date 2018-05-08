using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour {

    public Texture2D skillIcon;
    public Texture2D coolIcon;
    private IUserAction action;
    private float translationX;
    private float translationZ;
    private float coolTime = 0;
    // Use this for initialization
    void Start () {
        action = SSDirector.GetInstance().CurrentSceneController as IUserAction;
    }

    private void Update()
    {
        if (action.GetGameState() == GameState.GAMEOVER) return;
        //获取方向键的偏移量
        translationX = Input.GetAxis("Horizontal");
        translationZ = Input.GetAxis("Vertical");
        //移动玩家
        action.PlayerMove(new Vector3(translationX,0,translationZ));
        if (coolTime<=0 && Input.GetButtonDown("Jump"))//如果不在冷却时间内，按下空格可以触发加速技能
        {
            action.SpeedUp();
            coolTime = 10;
        }
        else
        {
            coolTime -= Time.deltaTime;
        }
    }
    private void FixedUpdate()
    {
        action.PlayerRotate(new Vector3(translationX, 0, translationZ));//玩家旋转方向
    }
    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 30;
        style.normal.textColor = Color.white;
        style.alignment = TextAnchor.UpperCenter;

        GUI.skin.button.fontSize = 25;

        GUIStyle style2 = new GUIStyle();
        style2.fontSize = 25;
        style2.normal.textColor = Color.white;
        style2.alignment = TextAnchor.MiddleCenter;

        if (coolTime <= 0)
        {
            GUI.Box(new Rect(Screen.width - 70, Screen.height - 70, 60, 60), skillIcon);
            GUI.Label(new Rect(Screen.width - 70, Screen.height - 70, 60, 60), "space", style2);
        }
        else
        {
            GUI.Box(new Rect(Screen.width - 70, Screen.height - 70, 60, 60), coolIcon);
            GUI.Label(new Rect(Screen.width - 70, Screen.height - 70, 60, 60), Mathf.Ceil(coolTime).ToString(),style2);
        }
        if (action.GetGameState()==GameState.GAMEOVER)
        {
            GUI.Box(new Rect(Screen.width / 2 - 70, Screen.height / 2 - 70, 140, 140), "GAMEOVER",style);
            if(GUI.Button(new Rect(Screen.width / 2 - 70, Screen.height / 2 - 20, 140, 40), "RESTART"))
            {
                Singleton<ScoreRecorder>.Instance.Reset();
                action.Restart();
            }
        }
        if (action.GetGameState() != GameState.NOT_RUN &&
            action.GetGameState() != GameState.GAMEOVER)
        {
            GUI.Box(new Rect(10, 10, 100, 50), "Score:"+
                Singleton<ScoreRecorder>.Instance.GetScore().ToString(),style);            
        }
    }
}
