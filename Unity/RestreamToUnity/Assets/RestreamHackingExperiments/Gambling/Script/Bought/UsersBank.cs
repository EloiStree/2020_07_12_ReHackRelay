using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RestreamChatHacking;
using System;

public class UsersBank : MonoBehaviour {


    [SerializeField]
    private long minimumCash = 10000;
    private static UsersBank _instance;

    public static UsersBank Instrance
    {
        get { return _instance; }
        private set { _instance = value; }
    }


    private Dictionary<string, UserBankAccount> _users = new Dictionary<string, UserBankAccount>();

    [Header("Debug")]
    [SerializeField]
    private List<UserBankAccount> _usersRegistered = new List<UserBankAccount>();

    public void Awake()
    {
        _instance = this;
        Restream.Listener.AddNewMessageListener(UserMessageDetected);
    }

    private void UserMessageDetected(RestreamChatMessage newMessage)
    {
        UserIdentifier userId = newMessage.UserID;
        if (UserNotRegistered(userId.GetID()))
            CreateBankAccount(userId, minimumCash);
        
    }

    private bool UserNotRegistered(string userId)
    { return !UserRegistered(userId); }
    private bool UserRegistered(string userId)
    {
        return _users.ContainsKey(userId);
    }


    public void CreateBankAccount(UserIdentifier userId, long minimumCash)
    {
        UserBankAccount account = new UserBankAccount(userId, minimumCash);
        _users.Add(userId.GetID(),account);
        _usersRegistered.Add(account);
    }

    public bool AllowToUse(UserIdentifier user,long cashToUse) {
        UserBankAccount account = GetAccountOf(user);
        if (account == null) return false;
        return account.IsAllowToUse(cashToUse);
    }

    internal bool Use(object userID, long cost)
    {
        throw new NotImplementedException();
    }

    public bool Use(UserIdentifier user, long cashToUser) {

        UserBankAccount account = GetAccountOf(user);
        if (account == null)
            return false;
        return account.Use(cashToUser);
    }


    internal void Add(UserIdentifier user, long cashToUser)
    {
        UserBankAccount account = GetAccountOf(user);
        if (account == null)
            return ;
         account.Add(cashToUser);
    }



    public UserBankAccount GetAccountOf(UserIdentifier user)
    {
        return GetAccountOf(user.GetID());
    }
    public   UserBankAccount GetAccountOf(string userId)
    {
        if (UserNotRegistered(userId))
            return null;
        return _users[userId];
    }
}

[System.Serializable]
public class UserBankAccount {

    public UserBankAccount(UserIdentifier id, long cash) {
        _userId = id;
        _cash = cash;
    }

    [SerializeField]
    private UserIdentifier _userId;
    [SerializeField]
    private long _cash= 20000;

    public UserIdentifier UserID { get { return _userId; } }
    public long Cash { get{ return _cash; } }
    public bool IsAllowToUse(long cash) { return cash <= _cash; }
    public bool Use(long cash)
    {
        if (IsAllowToUse(cash))
        {
            _cash -= cash;
            return true;
        }
        return false;


    }

    internal void Add(long cashToUser)
    {
        _cash += cashToUser < 0 ? 0 : cashToUser;
    }
}