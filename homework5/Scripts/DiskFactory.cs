using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiskFactory : MonoBehaviour {

    public GameObject diskPrefab;
    private List<DiskData> used = new List<DiskData>();//存储已使用的飞碟
    private List<DiskData> free = new List<DiskData>();//存储空余的飞碟

    public GameObject GetDisk(int round)
    {
        GameObject newObject;
        if (free.Count > 0)//判断有无空余的飞碟，有则取空余的飞碟，没有的话就实例化
        {
            newObject = free[0].gameObject;
            free.Remove(free[0]);
            newObject.transform.position = new Vector3(-18, 0, 0);
            newObject.transform.localScale = new Vector3(2, 0.1f, 2);
        }
        else
        {
            newObject = Instantiate<GameObject>(diskPrefab, new Vector3(-18, 0, 0), Quaternion.identity);
        }
        int randomNum = Random.Range(0, 3);
        switch (randomNum)
        {
            case 0:
                newObject.GetComponent<DiskData>().color = Color.red;
                newObject.GetComponent<DiskData>().size = 1.5f;
                newObject.GetComponent<DiskData>().speed = Random.Range(2f, 4f)+round;
                newObject.GetComponent<DiskData>().direction = new Vector3(1, Random.Range(-0.3f, 0.3f), 0);
                break;
            case 1:
                newObject.GetComponent<DiskData>().color = Color.yellow;
                newObject.GetComponent<DiskData>().size = 1;
                newObject.GetComponent<DiskData>().speed = Random.Range(4f, 6f)+round;
                newObject.GetComponent<DiskData>().direction = new Vector3(1, Random.Range(-0.3f, 0.3f), 0);
                break;
            case 2:
                newObject.GetComponent<DiskData>().color = Color.gray;
                newObject.GetComponent<DiskData>().size = 0.8f;
                newObject.GetComponent<DiskData>().speed = Random.Range(6f, 8f)+round;
                newObject.GetComponent<DiskData>().direction = new Vector3(1, Random.Range(-0.3f, 0.3f), 0);
                break;
        }
        Vector3.Normalize(newObject.GetComponent<DiskData>().direction);
        newObject.GetComponent<Renderer>().material.color = newObject.GetComponent<DiskData>().color;
        newObject.transform.localScale *= newObject.GetComponent<DiskData>().size;
        newObject.SetActive(true);
        used.Add(newObject.GetComponent<DiskData>());
        return newObject;
    }
    public void FreeDisk(GameObject ob)//将不用的飞碟回收到空余区
    {
        DiskData disk = null;
        foreach(DiskData i in used)
        {
            if (ob == i.gameObject)
            {
                disk = i;
            }
        }
        if (disk != null)
        {
            free.Add(disk);
            used.Remove(disk);
            disk.gameObject.SetActive(false);
        }
    }
}
