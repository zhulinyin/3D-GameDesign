using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : NetworkBehaviour
{
    [SyncVar]
    public SyncListInt map = new SyncListInt();     //同步井字棋的状态
    public static int[] MAP = new int[9];           //同一个客户端共享此全局变量

    [SyncVar]
    public int round = 0;                           //记录井字棋的回合数
    public static int ROUND = 0;                    //同一个客户端共享此全局变量

    public enum State { win,fail,draw,normal};
    public State state;                             //游戏状态

    public Sprite cha;                              //叉的贴图
    public Sprite quan;                             //圈的贴图
    private Button[] buttons = new Button[9];       //井字棋的九个格
    
    public GUISkin mySkin;

    private void Start()
    {
        state = State.normal;                       //初始化游戏状态
        for(int i = 0; i < 9; i++)                  //初始化井字棋状态
        {
            map.Add(0);
        }
        for(int i = 1; i <= 9; i++)                 //获取九个按钮并添加点击事件
        {
            GameObject button = GameObject.Find("Canvas/" + i);
            buttons[i-1] = button.GetComponent<Button>();
            buttons[i-1].onClick.AddListener(delegate () {
                onClick(button);
            });
        }
    }

    private void Update()
    {
        if (isServer)                               //在服务端更新同步变量
        {
            round = ROUND;
            for(int i = 0; i < 9; i++)
            {
                map[i] = MAP[i];
            }
        }
    }
    private void OnGUI()
    {
        GUI.skin = mySkin;
        state = Update_state(isServer);             //更新游戏状态
        if(state == State.win)
        {
            GUI.Box(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 50, 200, 100), "你赢了");
        }
        else if(state == State.fail)
        {
            GUI.Box(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 50, 200, 100), "你输了");
        }
        else if(state == State.draw)
        {
            GUI.Box(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 50, 200, 100), "平局");
        }        
    }
    void onClick(GameObject button)
    {
        int i = int.Parse(button.name);
        if (!isLocalPlayer || map[i - 1] != 0) return;  //点击时判断是否时本地玩家以及该按钮是否已被点击过
        if (isServer && round % 2 == 0)                 //服务端发送命令
        {
            CmdSend(button, i);
        }
        else if (!isServer && round % 2 != 0)           //客户端发送命令
        {
            CmdSend(button, i);
        }
    }
    State Update_state(bool server)                     //更新服务端和客户端游戏状态
    {
        State s = State.normal;
        int x = 1;
        if ((map[0] == x && map[1] == x && map[2] == x) ||
            (map[3] == x && map[4] == x && map[5] == x) ||
            (map[6] == x && map[7] == x && map[8] == x) ||
            (map[0] == x && map[3] == x && map[6] == x) ||
            (map[1] == x && map[4] == x && map[7] == x) ||
            (map[2] == x && map[5] == x && map[8] == x) ||
            (map[0] == x && map[4] == x && map[8] == x) ||
            (map[2] == x && map[4] == x && map[6] == x))
        {
            if (server) s = State.win;
            else s = State.fail;
        }
        else if (round == 9) s = State.draw;
        x = 2;
        if ((map[0] == x && map[1] == x && map[2] == x) ||
            (map[3] == x && map[4] == x && map[5] == x) ||
            (map[6] == x && map[7] == x && map[8] == x) ||
            (map[0] == x && map[3] == x && map[6] == x) ||
            (map[1] == x && map[4] == x && map[7] == x) ||
            (map[2] == x && map[5] == x && map[8] == x) ||
            (map[0] == x && map[4] == x && map[8] == x) ||
            (map[2] == x && map[4] == x && map[6] == x))
        {
            if (server) s = State.fail;
            else s = State.win;
        }
        else if (round == 9) s = State.draw;
        return s;
    }
    [Command]
    void CmdSend(GameObject button, int i)      //更新全局变量并向所有客户端发送消息
    {
        RpcUpdate(button, round % 2 == 0);
        ROUND++;
        MAP[i - 1] = round % 2 == 0 ? 1 : 2;
    }
    [ClientRpc]
    void RpcUpdate(GameObject button, bool circle)      //更新井字棋界面
    {
        if (circle)
        {
            button.GetComponent<Image>().sprite = quan;
        }
        else
        {
            button.GetComponent<Image>().sprite = cha;
        }
    }
}
