using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstController : MonoBehaviour,ISceneController {

    SSDirector director;
    public SSActionManager actionManager { get; set; }
    public GUISkin skin;

    public enum BoatPosition { left,right};
    public BoatPosition boatPosition = BoatPosition.left;//船的位置

    public enum GameState { win,fail,stop,start};
    public GameState state = GameState.stop;//游戏状态

    public List<GameObject> Left = new List<GameObject>();//左岸的人物
    public List<GameObject> Right = new List<GameObject>();//右岸的人物
    public List<GameObject> OnTheBoat = new List<GameObject>();//船上的人物

    public GameObject boat;//船
    public bool BoatMove = false;//记录船是否正在移动
    public GameObject PlayerMove = null;//记录被点击的人物
    public GameObject pos1, pos2;//记录船上两个位置分别对应的人物
    void Awake()
    {
        director = SSDirector.GetInstance();
        director.CurrentSceneController = this;
    }
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
      
    }

    public void LoadResources()//加载游戏资源
    {
        Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/River"),
            new Vector3(0,-4,0),Quaternion.identity);
        Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Ground"),
            new Vector3(-9, -3, 0), Quaternion.identity);
        Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Ground"),
            new Vector3(9, -3, 0), Quaternion.identity);
        boat = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Boat"),
            new Vector3(-4,-1,0), Quaternion.identity);
        for (int i = 0; i < 3; i++)
        {
            Left.Add(Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Priest"),
            new Vector3(-11 + i, 0, 0), Quaternion.identity));
            Left[i].name = i.ToString();                
        }
        for (int i = 0; i < 3; i++)
        {
            Left.Add(Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Devil"),
            new Vector3(-8 + i, 0, 0), Quaternion.identity));
            Left[i + 3].name = (i + 3).ToString();
        }
    }
    public void Pause()
    {

    }
    public void Resume()
    {
        
    }
    private void OnGUI()
    {
        GUI.skin = skin;
        if (state==GameState.stop)//如果游戏状态为停止，则弹出游戏介绍框
        {
            GUI.Box(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 150, 300, 300), 
                "Help the Priests and Devils to cross the river.The red spheres are Devils, " +
                "and the green spheres are Priests. If the Priests are out numbered by " +
                "the Devils on either side of the river they get killed.");
            if(GUI.Button(new Rect(Screen.width / 2 - 25, Screen.height / 2 + 100, 50, 30), "PLAY"))
            {
                state = GameState.start;
                director.CurrentSceneController.LoadResources();
            }
        }
        else
        {
            if (GUI.Button(new Rect(Screen.width / 2 - 25, Screen.height / 2 - 150, 50, 50), "GO"))
            {
                if (OnTheBoat.Count != 0)
                {
                    BoatMove = true;
                }
            }
        }
        if (state == GameState.fail)//失败状态
        {
            GUI.Box(new Rect(Screen.width / 2 - 80, Screen.height / 2 - 50, 160, 100),"fail!");
            if (GUI.Button(new Rect(Screen.width / 2 - 40, Screen.height / 2 + 15, 80, 30), "RESTART"))
            {
                director.CurrentSceneController.Restart();
            }
        }
        else if (state == GameState.win)//胜利状态
        {
            GUI.Box(new Rect(Screen.width / 2 - 80, Screen.height / 2 - 50, 160, 100), "win!");
            if (GUI.Button(new Rect(Screen.width / 2 - 40, Screen.height / 2 + 15, 80, 30), "RESTART"))
            {
                director.CurrentSceneController.Restart();
            }
        }
    }
    public void CheckState1(List<GameObject> direction)//检查船出发时该岸的恶魔是否多于牧师
    {        
        int numOfDevils = 0;
        int numOfPriests = 0;
        for(int i = 0; i < direction.Count; i++)
        {
            if(direction[i].transform.tag == "priest")
            {
                numOfPriests++;
            }
            else if(direction[i].transform.tag == "devil")
            {
                numOfDevils++;
            }
        }
        if (numOfDevils > numOfPriests && numOfPriests != 0) state = GameState.fail;
    }
    public void CheckState2(List<GameObject> direction)//检查船到岸时该岸和船上的恶魔是否多余牧师
    {
        int numOfDevils = 0;
        int numOfPriests = 0;
        for (int i = 0; i < direction.Count; i++)
        {
            if (direction[i].transform.tag == "priest")
            {
                numOfPriests++;
            }
            else if (direction[i].transform.tag == "devil")
            {
                numOfDevils++;
            }
        }
        for(int i = 0; i < OnTheBoat.Count; i++)
        {
            if (OnTheBoat[i].transform.tag == "priest")
            {
                numOfPriests++;
            }
            else if (OnTheBoat[i].transform.tag == "devil")
            {
                numOfDevils++;
            }
        }
        if (numOfDevils > numOfPriests && numOfPriests != 0) state = GameState.fail;
    }
    public void Restart()//重新开始游戏
    {
        SceneManager.LoadScene("main");
    }
}
