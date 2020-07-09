
using System;
using UnityEngine;

[Serializable]
public struct UserIdentifier
{


    public string GetID()
    {
        return _platformId + ":" + _userName;
    }

    public UserIdentifier(ChatPlatform platform, string userName)
    {
        _platformId = platform;
        _userName = userName;

    }


    [SerializeField]
    private ChatPlatform _platformId;

    public ChatPlatform Platform
    {
        get { return _platformId; }
        private set { _platformId = value; }
    }
    [SerializeField]
    private string _userName;

    public string UserName
    {
        get { return _userName; }
        private set { _userName = value; }
    }

    public override string ToString()
    {
        return string.Format("{0}:{1}", UserName, Platform);
    }

    public static bool operator ==(UserIdentifier obj1, UserIdentifier obj2)
    {
        if (ReferenceEquals(obj1, obj2))
        {
            return true;
        }

        if (ReferenceEquals(obj1, null))
        {
            return false;
        }
        if (ReferenceEquals(obj2, null))
        {
            return false;
        }

        return (obj1.UserName == obj2.UserName
                && obj1.Platform == obj2.Platform);
    }

    // this is second one '!='
    public static bool operator !=(UserIdentifier obj1, UserIdentifier obj2)
    {
        return !(obj1 == obj2);
    }

    public bool Equals(UserIdentifier other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }
        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Platform.Equals(other.Platform)
               && UserName.Equals(other.UserName);
    }

    public override bool Equals(object obj)
    {
        if (ReferenceEquals(null, obj))
        {
            return false;
        }
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        return obj.GetType() == GetType() && Equals((UserIdentifier)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            int hashCode = Platform.GetHashCode();
            hashCode = (hashCode * 397) ^ UserName.GetHashCode(); ;
            return hashCode;
        }
    }


}