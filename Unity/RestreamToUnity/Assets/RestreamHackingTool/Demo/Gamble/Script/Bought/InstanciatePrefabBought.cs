using System;
using System.Collections;
using System.Collections.Generic;
using RestreamChatHacking;
using UnityEngine;

public class InstanciatePrefabBought : MonoBehaviour {


    public UsersBuyRequestManager _buyManager;
    public Transform []_whereToSpawn;


	void Awake () {
        _buyManager._onPrefabBought.AddListener(CreateBoughtObject);
		
	}

    private void CreateBoughtObject(string id , long cost , GameObject prefab, RestreamChatMessage who)
    {
        Transform where = GetRandomPosition();
        if (where == null)
            return;
        GameObject gamo =  Instantiate(prefab, where.position, where.rotation);
        gamo.name = string.Format("#{0}({1})", id, who.UserName);
    }

    private Transform GetRandomPosition()
    {
        if (_whereToSpawn.Length <= 0) return null;
        return _whereToSpawn[UnityEngine.Random.Range(0, _whereToSpawn.Length - 1)];
    }
}
