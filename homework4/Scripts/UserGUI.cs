using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserGUI : MonoBehaviour {

    private IUserAction action;
	// Use this for initialization
	void Start () {
        action = SSDirector.GetInstance().CurrentSceneController as IUserAction;
	}

    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 30;
        style.normal.textColor = Color.white;
        style.alignment = TextAnchor.UpperCenter;
        GUI.skin.button.fontSize = 25;
        if (Input.GetMouseButtonDown(0))
        {
            action.Hit(Input.mousePosition);
        }
        if (action.GetGameState() == GameState.NOT_RUN &&
            GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 20, 100, 40), "START"))
        {
            action.SetGameState(GameState.ROUND_START);
        }
        if(action.GetGameState()==GameState.GAMEOVER)
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
            GUI.Box(new Rect(Screen.width / 2-50, 10, 100, 50), "Round:" +
                action.GetRound().ToString(), style);
        }
    }
}
