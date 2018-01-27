using System;
using System.Collections;
using System.Collections.Generic;
using RestreamChatHacking;
using UnityEngine;

public class InstanciatePrefabBough_Direct : InstanciatePrefabBought
{
    public override void ObjectToInstanciateWhenItMust(Bought bought)
    {
        InstanciateRandomlyTheObject(bought);
    }
}
