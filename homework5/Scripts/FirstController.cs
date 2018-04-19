using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FirstController : MonoBehaviour,ISceneController,IUserAction {

    SSDirector director;
    public IActionManager actionManager { get; set; }
    public List<GameObject> Disks;
    private int round;
    private GameState gameState;
    private float RoundGapTime;
    private float DiskGapTime;
    private int diskNum;
    void Awake()
    {
        director = SSDirector.GetInstance();
        director.CurrentSceneController = this;
        director.CurrentSceneController.LoadResources();
    }
	// Use this for initialization
	void Start () {
        round = 1;
        gameState = GameState.NOT_RUN;
        RoundGapTime = 0;
        diskNum = 10;
        DiskGapTime = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if (gameState == GameState.NOT_RUN) return;//游戏未开始
        if(gameState == GameState.GAMEOVER)//游戏结束，回收所有飞碟
        {
            for(int i = Disks.Count - 1; i >= 0; i--)
            {
                Singleton<DiskFactory>.Instance.FreeDisk(Disks[i]);
                Disks.Remove(Disks[i]);
            }
        }
        if (Singleton<ScoreRecorder>.Instance.GetScore() < 0)//如果分数小于0，那么游戏结束
        {
            GameOver();
        }
        if (gameState == GameState.ROUND_OVER && RoundGapTime < 3)//每个回合之间相隔3秒
        {
            RoundGapTime += Time.deltaTime;
        }
        else if (gameState == GameState.ROUND_OVER && RoundGapTime >= 3)
        {
            RoundGapTime = 0;
            gameState = GameState.ROUND_START;
        }
        if (gameState == GameState.ROUND_START && diskNum > 0 && DiskGapTime < 1)//飞碟的发射间隔为1秒
        {
            DiskGapTime += Time.deltaTime;
        }
        else if (gameState == GameState.ROUND_START && DiskGapTime >= 1 && diskNum > 0)
        {
            DiskGapTime = 0;
            Disks.Add(Singleton<DiskFactory>.Instance.GetDisk(round));
            actionManager.playDisk(Disks[Disks.Count-1]);
            diskNum--;
        }
        if (diskNum == 0 && Disks.Count==0)//当所有飞碟发射完毕且均被回收，那么回合结束
        {
            gameState = GameState.ROUND_OVER;
            diskNum = 10;
            round++;
        }
        for(int i = Disks.Count - 1;i >= 0;i--)//当飞碟超出屏幕的一定范围则被回收
        {
            if (Disks[i].transform.position.x > 20 || Disks[i].transform.position.y < -12 || 
                Disks[i].transform.position.y > 12)
            {
                Singleton<DiskFactory>.Instance.FreeDisk(Disks[i]);
                Singleton<ScoreRecorder>.Instance.SubScore();
                Disks.Remove(Disks[i]);
            }
        }
    }
    public int GetRound()//获取当前回合数
    {
        return round;
    }
    public void LoadResources()//加载游戏资源
    {
        Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Background"));        
    }
    public void Pause()
    {

    }
    public void Resume()
    {
        
    }
    
    public void Restart()//重新开始游戏
    {
        round = 1;
        SetGameState(GameState.ROUND_START);
    }
    public void Hit(Vector3 pos)
    {       
        Ray ray = Camera.main.ScreenPointToRay(pos);//从相机处发出射线
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject.tag.Contains("Finish"))//判断射线击中点是否是飞碟
            {
                Singleton<DiskFactory>.Instance.FreeDisk(hit.collider.gameObject);
                Disks.Remove(hit.collider.gameObject);
                Singleton<ScoreRecorder>.Instance.AddScore(hit.collider.gameObject.GetComponent<Renderer>().material.color);
            }
        }        
    }
    public GameState GetGameState()//获取当前游戏状态
    {
        return gameState;
    }
    public void SetGameState(GameState gameState)//设置当前游戏状态
    {
        this.gameState = gameState;
    }
    public void GameOver()//游戏结束
    {
        gameState = GameState.GAMEOVER;
    }
}
