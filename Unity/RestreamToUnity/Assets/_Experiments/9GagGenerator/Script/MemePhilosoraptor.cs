using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemePhilosoraptor : MonoBehaviour {


    [Header("Params")]
    public string _top;
    public string _bot;

    [Header("Display")]
    public Text _topText;
    public Text _botText;



    public void SetText(string top, string bot) {
        if(_topText!=null)
            _topText.text = _top = top;
        if (_botText != null)
            _botText.text =_bot = bot;
    }


    private void OnValidate()
    {

        SetText(_top, _bot);
    }
}
