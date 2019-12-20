using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MarchingBytes;

public class ObjectPool : MonoBehaviour
{
    public string poolName;

	List<GameObject> goList = new List<GameObject>();

	public GameObject CreateFromPoolAction(Vector3 spawnLocation)	{
		GameObject go = EasyObjectPool.instance.GetObjectFromPool(poolName, spawnLocation, Quaternion.identity);
		if (go)
		{
			goList.Add(go);
		}
        return go;
	}

    public void ReturnToPoolAction(GameObject returnObject)
    {
        EasyObjectPool.instance.ReturnObjectToPool(returnObject);
        goList.Remove(returnObject);
    }

    public void ReturnAllToPoolAction()
	{
		foreach (GameObject go in goList)
		{
			EasyObjectPool.instance.ReturnObjectToPool(go);
		}
		goList.Clear();
	}
}
