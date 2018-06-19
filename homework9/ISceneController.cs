using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISceneController
{
    void LoadResources();
    void Pause();
    void Resume();
    void Restart();
    GameObject GetPlayer();
}
