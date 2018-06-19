using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserGUI : MonoBehaviour {

    private IUserAction action;
    private float translationX;
    private float translationZ;
    private float m_MovementInputValue;
    private float m_TurnInputValue;
    // Use this for initialization
    void Start () {
        action = SSDirector.GetInstance().CurrentSceneController as IUserAction;
    }

    private void Update()
    {
        m_MovementInputValue = Input.GetAxis("Vertical");
        m_TurnInputValue = Input.GetAxis("Horizontal");
        
        if (Input.GetButtonUp("Fire1"))
        {
            action.shoot();
        }
    }

    private void FixedUpdate()
    {
        action.Move(m_MovementInputValue);
        action.Turn(m_TurnInputValue);
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
