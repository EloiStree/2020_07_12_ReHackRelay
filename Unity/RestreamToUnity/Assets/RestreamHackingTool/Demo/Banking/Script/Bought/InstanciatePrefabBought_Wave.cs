using System;
using System.Collections;
using System.Collections.Generic;
using RestreamChatHacking;
using UnityEngine;

public class InstanciatePrefabBought_Wave : InstanciatePrefabBought
{

    public int _waveCount;
    public float _waveDuration=60f;
    public float _currentWaveCountDown=30f;
    public List<Bought> _toCreate = new List<Bought>();


    public void Update() {
        _currentWaveCountDown -= Time.deltaTime;
        if (_currentWaveCountDown < 0)
        {
            NewWave();
        }

    }

    private void NewWave()
    {
        _currentWaveCountDown = _waveDuration;
        _waveCount++;
        ReleaseTheWave();
    }

    private void ReleaseTheWave()
    {

        for (int i = _toCreate.Count-1; i >=0; i--)
        {

            InstanciateRandomlyTheObject(_toCreate[i]);
            _toCreate.RemoveAt(i);
        }
        
    }

    public override void ObjectToInstanciateWhenItMust(Bought bought)
    {
        _toCreate.Add(bought);

    }
}