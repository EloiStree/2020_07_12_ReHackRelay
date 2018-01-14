using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlatformCountToUI : MonoBehaviour {

    public MessagesByPlatformCount _platformCount;

    public Text _youtubeCount;
    public Text _twitchCount;
    public Text _facebookCount;

    void Start () {
        InvokeRepeating("Refresh", 0, 0.25f);
	}

	void Refresh ()
    {
        _youtubeCount.text = ""+ _platformCount._youtubeCount;
        _twitchCount.text = "" + _platformCount._twitchCount;
        _facebookCount.text = "" + _platformCount._facebookCount;

    }
}
