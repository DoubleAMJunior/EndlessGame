using UnityEngine.UI;

public class MultiLanguageText : Text
{
    public string label;

    protected override void OnEnable()
    {
        base.OnEnable();
        LanguageManager.changeLanguage += setup;
        setup();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        LanguageManager.changeLanguage -= setup;
    }

    public void setup()
    {
        text=LanguageManager.Instance.getValue(label);
        if(LanguageManager.Instance.currentFont!=null)
            font = LanguageManager.Instance.currentFont;
    }
}
