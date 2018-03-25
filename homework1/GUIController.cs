using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GUIController : MonoBehaviour
{
    private int[,] map = new int[3, 3];
    enum State { win,fail,draw,normal};
    State state;
    List<int> arr = new List<int>();
    public GUISkin mySkin;
    public Texture2D cha;
    public Texture2D quan;
    static public Texture2D none;
    private Texture2D[,] background = new Texture2D[3, 3];
    private bool myTurn;
    private bool have_choose;
    private void Start()
    {
        for(int i = 0; i < 9; i++)
        {
            arr.Add(i);
        }
        state = State.normal;
        for(int i = 0; i < 3; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                map[i, j] = 0;
                background[i, j] = none;
            }
        }
        have_choose = false;
    }
    void OnGUI()
    {
        GUI.skin = mySkin;
        GUI.BeginGroup(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 180, 300, 400));
        if (!have_choose)
        {
            GUI.Box(new Rect(0, 0, 300, 300), "请选择出手顺序");
            if(GUI.Button(new Rect(50, 100, 200, 30), "先手"))
            {
                have_choose = true;
                myTurn = true;
            }
            if(GUI.Button(new Rect(50, 150, 200, 30), "后手"))
            {
                have_choose = true;
                myTurn = false;
            }
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (GUI.Button(new Rect(i * 100, j * 100, 100, 100), background[i, j]) && myTurn && map[i, j] == 0 &&
                        state==State.normal)
                    {
                        map[i, j] = 1;
                        background[i, j] = cha;
                        arr.Remove(i * 3 + j);
                        myTurn = false;
                        state = Update_state(1);
                    }
                    else if (!myTurn && state==State.normal)
                    {
                        int index = Calculate_index();
                        map[index / 3, index % 3] = 2;
                        background[index / 3, index % 3] = quan;
                        arr.Remove(index);
                        myTurn = true;
                        state = Update_state(2);
                    }
                }
            }            
            if(state!=State.normal)
            {
                string message=null;
                if (state == State.win)
                {
                    message = "你赢了！";
                }
                else if (state == State.fail)
                {
                    message = "你输了！";
                }
                else if (state == State.draw)
                {
                    message = "平局！";
                }
                GUI.Box(new Rect(80, 100, 140, 50), message);
                
            }
            if (GUI.Button(new Rect(80, 300, 140, 30), "重新开始"))
            {
                SceneManager.LoadScene("main");
            }
        }
        
        GUI.EndGroup();
    }
    State Update_state(int x)
    {
        State s = State.normal;
        if ((map[0, 0] == x && map[0, 1] == x && map[0, 2] == x) ||
            (map[1, 0] == x && map[1, 1] == x && map[1, 2] == x) ||
            (map[2, 0] == x && map[2, 1] == x && map[2, 2] == x) ||
            (map[0, 0] == x && map[1, 0] == x && map[2, 0] == x) ||
            (map[0, 1] == x && map[1, 1] == x && map[2, 1] == x) ||
            (map[0, 2] == x && map[1, 2] == x && map[2, 2] == x) ||
            (map[0, 0] == x && map[1, 1] == x && map[2, 2] == x) ||
            (map[0, 2] == x && map[1, 1] == x && map[2, 0] == x))
        {
            if (x == 1) s = State.win;
            else if (x == 2) s = State.fail;
        }
        else if (arr.Count == 0) s = State.draw;
        return s;
    }
    int Calculate_index()
    {
        int best_index= arr[(int)Random.Range(0, arr.Count)]; ;
        for(int i = 0; i < arr.Count; i++)
        {
            if (Check_state(arr[i]) == State.fail)
            {
                return arr[i];
            }
            if(Check_state(arr[i]) == State.win)
            {
                best_index = arr[i];
            }
        }
        return best_index;
    }
    State Check_state(int index)
    {
        map[index / 3, index % 3] = 2;
        if (Update_state(2) == State.fail)
        {
            map[index / 3, index % 3] = 0;
            return State.fail;
        }
        map[index / 3, index % 3] = 1;
        if (Update_state(1) == State.win)
        {
            map[index / 3, index % 3] = 0;
            return State.win;
        }
        map[index / 3, index % 3] = 0;
        return State.normal;
    }
}
