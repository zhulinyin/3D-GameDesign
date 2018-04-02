﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstController : MonoBehaviour,ISceneController {

    SSDirector director;
    public GUISkin skin;

    enum BoatPosition { left,right};
    private BoatPosition boatPosition = BoatPosition.left;//船的位置

    enum GameState { win,fail,stop,start};
    GameState state = GameState.stop;//游戏状态

    private List<GameObject> Left = new List<GameObject>();//左岸的人物
    private List<GameObject> Right = new List<GameObject>();//右岸的人物
    private List<GameObject> OnTheBoat = new List<GameObject>();//船上的人物

    private GameObject boat;//船
    private bool BoatMove = false;//记录船是否正在移动
    private GameObject PlayerMove = null;//记录被点击的人物
    private GameObject pos1, pos2;//记录船上两个位置分别对应的人物
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
        if (state == GameState.fail || state == GameState.win) return;//如果游戏胜利或者失败就不再更新
        if (Input.GetMouseButtonDown(0) && !BoatMove)//如果船不在移动，判断是否点击鼠标左键
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//从相机处发出射线
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit))
            {
                if (hit.collider.gameObject.tag == "devil"|| hit.collider.gameObject.tag =="priest")//判断射线击中点是否是恶魔或者牧师
                {
                    PlayerMove = hit.collider.gameObject;//记录下被点击的人物
                }
            }
        }
        if (PlayerMove != null && PlayerMove.transform.parent == null)//被点击人物不为空且其父对象为空，说明被点击人物在岸上
        {
            if (((boatPosition == BoatPosition.left && Left.Contains(PlayerMove))||
                (boatPosition == BoatPosition.right && Right.Contains(PlayerMove)))
                && OnTheBoat.Count < 2)//判断船和被点击人物是否在同一岸边且船未坐满
            {
                if (pos1 == null)//如果船的第一个位置不为空则把人物移动到第一个位置
                {
                    PlayerMove.transform.position = boat.transform.position +
                        new Vector3(-0.5f, 1, 0);
                    pos1 = PlayerMove;
                }
                else//否则把人物移动到第二个位置
                {
                    PlayerMove.transform.position = boat.transform.position +
                        new Vector3(0.5f, 1, 0);
                    pos2 = PlayerMove;
                }
                PlayerMove.transform.parent = boat.transform;//将船设为人物的父对象
                if(boatPosition == BoatPosition.left)//从对应的岸中移除人物
                {
                    Left.Remove(PlayerMove);
                }
                else if (boatPosition == BoatPosition.right)
                {
                    Right.Remove(PlayerMove);
                }
                OnTheBoat.Add(PlayerMove);//将人物添加到船上
            }
            PlayerMove = null;//将点击的人物清空
        }
        if(PlayerMove != null && PlayerMove.transform.parent != null)//被点击的人物不为空且其父对象不为空，说明人物在船上
        {
            if (boatPosition == BoatPosition.right)//判断船的位置
            {
                PlayerMove.transform.position = new Vector3(5.5f + Convert.ToInt32(PlayerMove.name), 0, 0);
                PlayerMove.transform.parent = null;
                Right.Add(PlayerMove);
                if (Right.Count == 6) state = GameState.win;//如果右岸人数为6，则游戏胜利
            }
            else if (boatPosition == BoatPosition.left)
            {
                PlayerMove.transform.position = new Vector3(-11 + Convert.ToInt32(PlayerMove.name), 0, 0);
                PlayerMove.transform.parent = null;
                Left.Add(PlayerMove);
            }
            if (pos1 == PlayerMove) pos1 = null;
            else pos2 = null;
            OnTheBoat.Remove(PlayerMove);
            PlayerMove = null;
        }
        if (BoatMove)//判断船是否在移动
        {
            if (boatPosition == BoatPosition.right)//如果船在右边则向左移动
            {
                CheckState1(Right);
                boat.transform.Translate(Vector3.left * Time.deltaTime * 3);
                if (boat.transform.position.x <= -4)
                {
                    BoatMove = false;
                    boatPosition = BoatPosition.left;
                    CheckState2(Left);
                }
            }
            if (boatPosition == BoatPosition.left)//如果船在左边则向右移动
            {
                CheckState1(Left);
                boat.transform.Translate(Vector3.right * Time.deltaTime * 3);
                if (boat.transform.position.x >= 4)
                {
                    BoatMove = false;
                    boatPosition = BoatPosition.right;
                    CheckState2(Right);
                }
            }
        }
        
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
    private void CheckState1(List<GameObject> direction)//检查船出发时该岸的恶魔是否多于牧师
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
    private void CheckState2(List<GameObject> direction)//检查船到岸时该岸和船上的恶魔是否多余牧师
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
