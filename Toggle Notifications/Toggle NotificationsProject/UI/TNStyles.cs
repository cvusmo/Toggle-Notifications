using ToggleNotifications.TNTools.UI;
using UnityEngine;
using ToggleNotifications;

namespace ToggleNotifications.UI;

public static bool SquareButton(string txt)
{
    return GUILayout.Button(txt, TNStyle.big_button, GUILayout.Height(SquareButton_size), GUILayout.Width(SquareButton_size));
}

public static bool SquareButton(Texture2D icon)
{
    return GUILayout.Button(icon, TNStyle.big_button, GUILayout.Height(SquareButton_size), GUILayout.Width(SquareButton_size));
}

public class TNStyles : TNStyle
{
    private static bool guiLoaded = false;

    public static bool Init()
    {
        status_ok = new GUIStyle(label);
        status_ok.normal.textColor = new Color(0, 1, 0, 1);

        status_warning = new GUIStyle(label);
        status_warning.normal.textColor = new Color(1, 1, 0, 1);

        status_error = new GUIStyle(label);
        status_error.normal.textColor = new Color(1, 0, 0, 1);

        if (guiLoaded)
            return true;

        if (!TNStyle.Init())
            return false;

        guiLoaded = true;
        return true;
    }

    public static GUIStyle textInputStyle = new GUIStyle(GUI.skin.textField);

    public static Texture2D k2d2_big_icon;
    public static Texture2D mnc_icon;
    public static Texture2D icon;

    public static GUIStyle status;
    public static GUIStyle status_ok;
    public static GUIStyle status_style;
    public static GUIStyle status_warning;
    public static GUIStyle status_error;

    const int SquareButton_size = 60;
    internal static readonly GUISkin skin;

    static int spacingAfterHeader = 5;

    public static void DrawSectionHeader(string sectionName, string value = "", float labelWidth = -1, GUIStyle valueStyle = null)
    {
        if (valueStyle == null) valueStyle = TNStyle.label;

        GUILayout.BeginHorizontal();

        if (labelWidth < 0)
            GUILayout.Label($"<b>{sectionName}</b> ");
        else
            GUILayout.Label($"<b>{sectionName}</b> ", GUILayout.Width((int)labelWidth));

        GUILayout.FlexibleSpace();
        GUILayout.Label(value, valueStyle);
        GUILayout.Space(5);
        GUILayout.EndHorizontal();
        GUILayout.Space(spacingAfterHeader);
    }
}