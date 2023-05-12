
using ToggleNotifications.TNTools.UI;
using UnityEngine;


namespace ToggleNotifications;

public class TNStyles
{
    private static bool guiLoaded = false;
    public static bool Init()
    {
        if (guiLoaded)
            return true;

        if (!TNBaseStyle.Init())
            return false;

        TNBaseStyle.Skin.window.fixedWidth = 350; // Must fit with max_width given to DrawTabs (TabsUI.cs)

        // Load specific Icon and style here
        //EXAMPLEBigIcon = AssetsLoader.LoadIcon("example_big_icon");
        tnicon = AssetsLoader.LoadIcon("tn_icon");


        Status = new GUIStyle(GUI.skin.GetStyle("Label"));
        Status.alignment = TextAnchor.MiddleLeft;
        Status.margin = new RectOffset(0, 0, 0, 0);
        Status.padding = new RectOffset(0, 0, 0, 0);

        guiLoaded = true;
        return true;
    }

    //public static Texture2D EXAMPLEBigIcon;
    public static Texture2D tnicon;

    public static GUIStyle Status;

    const int SquareButtonSize = 60;

    public static bool SquareButton(string txt)
    {
        return GUILayout.Button(txt, TNBaseStyle.BigButton, GUILayout.Height(SquareButtonSize), GUILayout.Width(SquareButtonSize));
    }

    public static bool SquareButton(Texture2D icon)
    {
        return GUILayout.Button(icon, TNBaseStyle.BigButton, GUILayout.Height(SquareButtonSize), GUILayout.Width(SquareButtonSize));
    }

    public static int SpacingAfterHeader = 5;
    public static int SpacingAfterSection = 5;
    public static int SpacingAfterEntry = 2;
    public static int SpacingAfterTallEntry = -3;

    public static GUISkin skin { get; internal set; }

    public static void DrawSectionHeader(string sectionName, string value = "", float labelWidth = -1, GUIStyle valueStyle = null) // was (string sectionName, ref bool isPopout, string value = "")
    {
        if (valueStyle == null) valueStyle = TNBaseStyle.Label;

        GUILayout.BeginHorizontal();

        if (labelWidth < 0)
            GUILayout.Label($"<b>{sectionName}</b> ");
        else
            GUILayout.Label($"<b>{sectionName}</b> ", GUILayout.Width(labelWidth));
        GUILayout.FlexibleSpace();
        GUILayout.Label(value, valueStyle);
        GUILayout.Space(5);
        GUILayout.EndHorizontal();
        GUILayout.Space(SpacingAfterHeader);
    }

}

