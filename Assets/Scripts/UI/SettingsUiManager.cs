using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsUiManager : MonoBehaviour
{

    public GameObject mainMenu;
    

    public void OnMusicToggle()
    {
        print("music");
        PlayerProfile.Instance.MusicOn = !PlayerProfile.Instance.MusicOn;
        if (PlayerProfile.Instance.MusicOn)
        {
            AudioManager._instance.startPlay();
        }
        else
        {
            AudioManager._instance.StopAllPlayers();
        }
    }

    public void OnSoundToggle()
    {
        PlayerProfile.Instance.SoundOn = !PlayerProfile.Instance.SoundOn;
    }

    public void OnControlsToggle()
    {
        print("Controlls");
        PlayerProfile.Instance.HoldControlsOn = !PlayerProfile.Instance.HoldControlsOn;
    }

    public void OnParticlesToggle()
    {
        PlayerProfile.Instance.ParticleEffectsOn = !PlayerProfile.Instance.ParticleEffectsOn;
    }

    public void BackToMainMenu()
    {
        mainMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    public void changeLanguage()
    {
        if (LanguageManager.Instance.language==LanguageManager.Language.english)
        {
            LanguageManager.Instance.setupLanguage(LanguageManager.Language.persian);
        }
        else
        {
            LanguageManager.Instance.setupLanguage(LanguageManager.Language.english);
        }

        LanguageManager.Instance.callLanguageChange();
        PlayerProfile.Instance.Language = LanguageManager.Instance.language;
        PlayerProfile.SavePlayerProfile();
    }

    public void ResetProgress()
    {
        //TODO write the code for progress restart here
    }
}
