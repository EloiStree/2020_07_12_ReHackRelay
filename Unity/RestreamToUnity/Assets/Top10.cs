using RestreamChatHacking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;

public class Top10 : MonoBehaviour {

    public List<Score> _top10 = new List<Score>();
    public Text _topTenDebug;

    [Serializable]
    public class Score
    {
        public string playerName;
        public RestreamChatMessage.ChatPlatform playerPlatform;
        public int score = 0;
    }

	// Use this for initialization
	public void AddGoodAnswer (RestreamChatMessage answer) {

        Score  playerScore = GetPlayer(answer);
        if (playerScore == null)
        {
            playerScore = new Score() { playerName = answer.UserName, playerPlatform = answer.Platform };
            _top10.Add(playerScore);
        }
        playerScore.score++;
        _top10 = _top10.OrderByDescending(p => p.score).Take(10).ToList();
        _topTenDebug.text = GetListOfBestPlayers();
	}

    private Score GetPlayer(RestreamChatMessage answer)
    {
        for (int i = 0; i < _top10.Count; i++)
        {
            if (_top10[i].playerName == answer.UserName && _top10[i].playerPlatform == answer.Platform)
            {
                return _top10[i];
            }
        }
        return null;
    }

    public string GetListOfBestPlayers() {
        string top = "";
        for (int i =0;  i<_top10.Count; i++)
        {
            top = top +  "" + _top10[i].playerName +"("+_top10[i].score+")"+ "  "  ;

        }
        return top;
    }
}
