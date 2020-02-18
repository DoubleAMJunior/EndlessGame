using System;
using System.Collections;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    private static LanguageManager _instance;

    public static LanguageManager Instance
    {
        get
        {
            if (_instance==null)
            {
                GameObject go=GameObject.Find("LanguageManager");
                _instance = go.GetComponent<LanguageManager>();
            }
            return _instance;
        }
    }

    public enum Language
    {
        english,persian
    }

    public Language language;
    public TextAsset persianJson;
    public TextAsset englishJson;
    public Font persianFont;
    public Font englishFont;
    [HideInInspector]  public Font currentFont;
    private LanguageModel model;

    public delegate void onChangeLanguage();

    public static event onChangeLanguage changeLanguage;

    public void callLanguageChange()
    {
        changeLanguage();
    }
    
    private void Awake()
    {
        language = PlayerProfile.Instance.Language;
        setupLanguage(language);
    }

    public void setupLanguage(Language newLang)
    {
        language = newLang;
        TextAsset currentLanguageJasonFile;
        if (language==Language.english)
        {
            currentFont = englishFont;
            currentLanguageJasonFile = englishJson;
        }
        else
        {
            currentFont = persianFont;
            currentLanguageJasonFile = persianJson;
        }
        model = JsonUtility.FromJson<LanguageModel>(currentLanguageJasonFile.ToString());
    }
    
    public string getValue(string label)
    {
        string ans=(string)model.GetType().GetField(label).GetValue(model);
        if (language == Language.english)
        {
            return ans;
        }
        ans = ans.faConvert();
        return ans;
    }
    
}

[Serializable]
public class LanguageModel
{
    public string test;
    public string Ratings;
    public string Shop;
    public string Friends;
    public string Settings;
    public string TapToPlay;
    public string Music;
    public string Sound;
    public string ControlType;
    public string Retry;
    public string Back;
    public string LangChange;
    public string Score;
    public string HighScore;
}