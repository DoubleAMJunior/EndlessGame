using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProfile
{
    #region Private Atributes 
    private static PlayerProfile _instance;
    private int _highScore;
    private bool _soundOn;
    private bool _musicOn;
    private bool _particleEffectsOn;
    private bool _holdControlsOn;
    private int _levelProgress;
    private string _playerName;
    private int _latestScore;
    private LanguageManager.Language _language;
    #endregion

    #region Getter and setters
    public static PlayerProfile Instance
    {
        get { return _instance ?? (_instance = LoadProfile()); }
    }

    public int HighScore
    {
        get => _highScore;
        set => _highScore = value;
    }

    public bool SoundOn
    {
        get => _soundOn;
        set => _soundOn = value;
    }
    
    public bool MusicOn
    {
        get => _musicOn;
        set => _musicOn = value;
    }

    public bool ParticleEffectsOn
    {
        get => _particleEffectsOn;
        set => _particleEffectsOn = value;
    }

    public bool HoldControlsOn
    { 
       get => _holdControlsOn;
       set => _holdControlsOn = value;
    }

    public int LevelProgress
    {
        get => _levelProgress;
        set => _levelProgress = value;
    }

    public string PlayerName
    {
        get => _playerName;
        set => _playerName = value;
    }

    public int LatestScore
    {
        get => _latestScore;
        set => _latestScore = value;
    }

    public LanguageManager.Language Language
    {
        get => _language;
        set => _language = value;
    }

    #endregion

    #region behaviour functions
    public static PlayerProfile LoadProfile()
    {
        if (!PlayerPrefs.HasKey("HasProfile") || PlayerPrefs.GetInt("HasProfile") == 0)
        {
            PlayerPrefs.SetInt("HasProfile",1);
            PlayerPrefs.Save();
            return NewPlayerProfile();
        }

        PlayerProfile playerProfile = new PlayerProfile();
        playerProfile._highScore = PlayerPrefs.GetInt(StaticNames.HighScore);
        playerProfile._musicOn = PlayerPrefs.GetInt(StaticNames.MusicOn) == 1;
        playerProfile._soundOn = PlayerPrefs.GetInt(StaticNames.SoundOn) == 1;
        playerProfile._holdControlsOn = PlayerPrefs.GetInt(StaticNames.HoldControlsOn) == 1;
        playerProfile._particleEffectsOn = PlayerPrefs.GetInt(StaticNames.ParticleEffectsOn) == 1;
        playerProfile._levelProgress = PlayerPrefs.GetInt(StaticNames.LevelProgress);
        playerProfile._latestScore = PlayerPrefs.GetInt(StaticNames.LatestScore);
        playerProfile._language = (LanguageManager.Language)PlayerPrefs.GetInt("Language");
        return playerProfile;
    }

    public static PlayerProfile NewPlayerProfile()
    {
        PlayerProfile playerProfile = new PlayerProfile();
        playerProfile._highScore = 0;
        playerProfile._soundOn = true;
        playerProfile._musicOn = true;
        playerProfile._holdControlsOn = true;
        playerProfile._particleEffectsOn = true;
        playerProfile._levelProgress = 5;
        playerProfile._latestScore = 0;
        return playerProfile;
    }

    public static void SavePlayerProfile()
    {
        PlayerPrefs.SetInt(StaticNames.HighScore, Instance._highScore);
        PlayerPrefs.SetInt(StaticNames.MusicOn, Instance.MusicOn ? 1 : 0);
        PlayerPrefs.SetInt(StaticNames.SoundOn, Instance.SoundOn ? 1 : 0);
        PlayerPrefs.SetInt(StaticNames.HoldControlsOn, Instance.HoldControlsOn ? 1 : 0);
        PlayerPrefs.SetInt(StaticNames.ParticleEffectsOn, Instance.ParticleEffectsOn ? 1 : 0);
        PlayerPrefs.SetInt(StaticNames.LevelProgress, Instance.LevelProgress);
        PlayerPrefs.SetInt(StaticNames.LatestScore, Instance.LatestScore);
        PlayerPrefs.SetInt("Language",(int)Instance.Language);
        PlayerPrefs.Save();
    }

    public bool CheckForHighscore()
    {
        if (LatestScore > HighScore)
        {
            HighScore = LatestScore;
            return true;
        }

        return false;
    }
    #endregion
}
