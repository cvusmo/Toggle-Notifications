using ToggleNotifications.TNTools.UI;
using UnityEngine;

namespace ToggleNotifications
{
    public class TNStyles
    {
        public static bool guiLoaded = false;
        public static Texture2D TNBigIcon;
        public static Texture2D TNIcon;
        public static GUIStyle Status;
        private const int SquareButtonSize = 60;
        public static int SpacingAfterHeader = 5;
        public static int SpacingAfterSection = 5;
        public static int SpacingAfterEntry = 2;
        public static int SpacingAfterTallEntry = -3;
        public static bool Init()
        {
            if (TNStyles.guiLoaded)
                return true;
            if (!TNBaseStyle.Init())
                return false;
            TNBaseStyle.Skin.window.fixedWidth = 350f;
            TNStyles.TNBigIcon = AssetsLoader.LoadIcon("tn_big_icon");
            TNStyles.TNIcon = AssetsLoader.LoadIcon("tn_icon_100");
            TNStyles.Status = new GUIStyle(GUI.skin.GetStyle("Label"));
            TNStyles.Status.alignment = TextAnchor.MiddleLeft;
            TNStyles.Status.margin = new RectOffset(0, 0, 0, 0);
            TNStyles.Status.padding = new RectOffset(0, 0, 0, 0);
            TNStyles.guiLoaded = true;
            return true;
        }
        public static bool SquareButton(string txt) => GUILayout.Button(txt, TNBaseStyle.BigButton, GUILayout.Height(60f), GUILayout.Width(60f));
        public static bool SquareButton(Texture2D icon) => GUILayout.Button((Texture)icon, TNBaseStyle.BigButton, GUILayout.Height(60f), GUILayout.Width(60f));
        public static void DrawSectionHeader(
          string sectionName,
          string value = "",
          float labelWidth = -1f,
          GUIStyle valueStyle = null)
        {
            if (valueStyle == null)
                valueStyle = TNBaseStyle.Label;
            GUILayout.BeginHorizontal();
            if ((double)labelWidth < 0.0)
                GUILayout.Label("<b>" + sectionName + "</b> ");
            else
                GUILayout.Label("<b>" + sectionName + "</b> ", GUILayout.Width(labelWidth));
            GUILayout.FlexibleSpace();
            GUILayout.Label(value, valueStyle);
            GUILayout.Space(5f);
            GUILayout.EndHorizontal();
            GUILayout.Space((float) TNStyles.SpacingAfterHeader);
        }
    }
}
