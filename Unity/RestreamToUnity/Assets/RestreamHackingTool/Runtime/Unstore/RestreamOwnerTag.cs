using RestreamChatHacking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestreamOwnerTag : MonoBehaviour
{

    [Header("Owner Info")]
    private UserIdentifier _owner;

    public UserIdentifier GetOwner() { return _owner; }

    public static void LinkTo(GameObject target, UserIdentifier message)
    {

        RestreamOwnerTag script = target.AddComponent<RestreamOwnerTag>();
        script._owner = message;
    }
    public static void LinkTo(Transform target, UserIdentifier message)
    {
        LinkTo(target.gameObject, message);

    }
}
public class RestreamLinkedMessage : MonoBehaviour
{

    [Header("Owner Info")]
    private RestreamChatMessage _message;

    public UserIdentifier GetOwner() { return _message.UserID; }
    public RestreamChatMessage GetMessage() { return _message; }

    public static void LinkTo(GameObject target, RestreamChatMessage message)
    {
        RestreamLinkedMessage script = target.AddComponent<RestreamLinkedMessage>();
        script._message = message;
    }
    public static void LinkTo(Transform target, RestreamChatMessage message)
    {
        LinkTo(target.gameObject, message);
    }

}
