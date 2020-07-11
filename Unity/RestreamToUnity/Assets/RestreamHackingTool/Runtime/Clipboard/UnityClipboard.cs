using UnityEngine;

public class UnityClipboard
{

    //https://flystone.tistory.com/138
    public static string Get()
    {
        TextEditor _textEditor = new TextEditor();
        _textEditor.Paste();
        return _textEditor.text;
    }
    public static void Set(string value)
    {
        TextEditor _textEditor = new TextEditor
        { text = value };
        _textEditor.OnFocus();
        _textEditor.Copy();
    }
}

