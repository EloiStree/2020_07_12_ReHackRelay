using System;
using System.Collections;
using System.Collections.Generic;
using RestreamChatHacking;
using UnityEngine;

public abstract class InstanciatePrefabBought : MonoBehaviour {


    public UsersBuyRequestManager _buyManager;
    public Transform []_whereToSpawn;
    public Vector3 _randomAdjustment;


	void Awake () {
        _buyManager._onPrefabBought.AddListener(ObjectToInstanciateWhenItMust);

		
	}

    public void ObjectToInstanciateWhenItMust(string id, long cost, GameObject prefab, RestreamChatMessage who) {
        ObjectToInstanciateWhenItMust(new Bought() { id = id, cost=cost, prefab=prefab, who=who });
    }

    public abstract void ObjectToInstanciateWhenItMust(Bought bought);

    private Transform GetRandomPosition()
    {
        if (_whereToSpawn.Length <= 0) return null;
        return _whereToSpawn[UnityEngine.Random.Range(0, _whereToSpawn.Length - 1)];
    }


    public void InstanciateRandomlyTheObject(Bought toCreate)
    {

        Transform where = GetRandomPosition();
        if (where == null)
            return;
        GameObject gamo = Instantiate(toCreate.prefab, where.position, where.rotation);
        gamo.name = string.Format("#{0}({1})", toCreate.id, toCreate.who.UserName);
    }

    [Serializable]
    public class Bought {
        public string id;
        public long cost;
        public GameObject prefab;
        public  RestreamChatMessage who;
    }
}
