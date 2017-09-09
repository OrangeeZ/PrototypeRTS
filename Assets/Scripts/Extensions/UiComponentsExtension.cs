using UnityEngine.UI;

public static class UiComponentsExtension  {

    public static int SetValue(this Text text, int currentValue, int value)
    {
        if (currentValue == value)
            return currentValue;
        var newValue = value.ToString();
        if (text.text != newValue)
            text.text = newValue;
        return value;
    }
	
}
