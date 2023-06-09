﻿using SpaceWarp.API.UI;
using UnityEngine;


namespace ToggleNotifications.TNTools.UI
{
    internal class TNBaseStyle
    {
        internal static GUISkin Skin;
        private static bool guiLoaded;
        internal static GUIStyle Error;
        internal static GUIStyle Warning;
        internal static GUIStyle Label;
        internal static GUIStyle MidText;
        internal static GUIStyle ConsoleText;
        internal static GUIStyle PhaseOk;
        internal static GUIStyle PhaseWarning;
        internal static GUIStyle PhaseError;
        internal static GUIStyle IconsLabel;
        internal static GUIStyle Title;
        internal static GUIStyle TextInputStyle;
        internal static GUIStyle NameLabelStyle;
        internal static GUIStyle ValueLabelStyle;
        internal static GUIStyle UnitLabelStyle;
        internal static string UnitColorHex;
        internal static GUIStyle Separator;
        internal static Texture2D Gear;
        internal static Texture2D Icon;
        internal static Texture2D TNIcon;
        internal static Texture2D Cross;
        internal static GUIStyle ProgressBarEmpty;
        internal static GUIStyle ProgressBarFull;
        internal static GUIStyle BigiconButton;
        internal static GUIStyle IconButton;
        internal static GUIStyle SmallButton;
        internal static GUIStyle BigButton;
        internal static GUIStyle Button;
        internal static GUIStyle CtrlButton;
        internal static GUIStyle TabNormal;
        internal static GUIStyle TabActive;
        internal static GUIStyle FoldoutClose;
        internal static GUIStyle FoldoutOpen;
        internal static GUIStyle Toggle;
        internal static GUIStyle ToggleError;


        internal static bool Init()
        {
            return BuildStyles();
        }
        internal static bool BuildStyles()
        {
            if (TNBaseStyle.guiLoaded)
                return true;
            TNBaseStyle.Skin = TNBaseStyle.CopySkin(Skins.ConsoleSkin);
            TNBaseStyle.BuildFrames();
            TNBaseStyle.BuildButtons();
            TNBaseStyle.BuildTabs();
            TNBaseStyle.BuildFoldout();
            TNBaseStyle.BuildToggle();
            TNBaseStyle.BuildIcons();
            TNBaseStyle.BuildLabels();
            guiLoaded = true;
            return true;
        }
        private static void BuildLabels()
        {
            IconsLabel = new GUIStyle(GUI.skin.GetStyle("Label"))
            {
                border = new RectOffset(0, 0, 0, 0),
                padding = new RectOffset(0, 0, 0, 0),
                margin = new RectOffset(0, 0, 0, 0),
                overflow = new RectOffset(0, 0, 0, 0)
            };
            TNBaseStyle.Error = new GUIStyle(GUI.skin.GetStyle("Label"));
            TNBaseStyle.Warning = new GUIStyle(GUI.skin.GetStyle("Label"));
            TNBaseStyle.Error.normal.textColor = Color.red;
            TNBaseStyle.Warning.normal.textColor = Color.yellow;
            TNBaseStyle.PhaseOk = new GUIStyle(GUI.skin.GetStyle("Label"));
            TNBaseStyle.PhaseOk.normal.textColor = ColorTools.ParseColor("#00BC16");
            TNBaseStyle.PhaseWarning = new GUIStyle(GUI.skin.GetStyle("Label"));
            TNBaseStyle.PhaseWarning.normal.textColor = ColorTools.ParseColor("#BC9200");
            TNBaseStyle.PhaseError = new GUIStyle(GUI.skin.GetStyle("Label"));
            TNBaseStyle.PhaseError.normal.textColor = ColorTools.ParseColor("#B30F0F");
            TNBaseStyle.ConsoleText = new GUIStyle(GUI.skin.GetStyle("Label"));
            TNBaseStyle.ConsoleText.normal.textColor = ColorTools.ParseColor("#B6B8FA");
            TNBaseStyle.ConsoleText.padding = new RectOffset(0, 0, 0, 0);
            TNBaseStyle.ConsoleText.margin = new RectOffset(0, 0, 0, 0);
            TNBaseStyle.Label = new GUIStyle(GUI.skin.GetStyle("Label"))
            {
                margin = new RectOffset(0, 0, 0, 0),
                padding = new RectOffset(0, 0, 0, 0)
            };
            TNBaseStyle.Title = new GUIStyle();
            TNBaseStyle.Title.normal.textColor = ColorTools.ParseColor("#C0C1E2");
            TNBaseStyle.TextInputStyle = new GUIStyle(GUI.skin.GetStyle("textField"))
            {
                alignment = TextAnchor.LowerCenter,
                padding = new RectOffset(10, 10, 0, 0),
                contentOffset = new Vector2(0.0f, 2f),
                fixedHeight = 18f,
                fixedWidth = 90f,
                clipping = TextClipping.Overflow,
                margin = new RectOffset(0, 0, 2, 0)
            };
            TNBaseStyle.NameLabelStyle = new GUIStyle(GUI.skin.GetStyle("Label"));
            TNBaseStyle.NameLabelStyle.border = new RectOffset(0, 0, 5, 5);
            TNBaseStyle.NameLabelStyle.padding = new RectOffset(0, 0, 4, 4);
            TNBaseStyle.NameLabelStyle.overflow = new RectOffset(0, 0, 0, 0);
            TNBaseStyle.ValueLabelStyle = new GUIStyle(GUI.skin.GetStyle("Label"))
            {
                alignment = TextAnchor.MiddleRight
            };
            TNBaseStyle.UnitLabelStyle = new GUIStyle(ValueLabelStyle)
            {
                fixedWidth = 24f,
                alignment = TextAnchor.MiddleLeft
            };
            TNBaseStyle.UnitLabelStyle.normal.textColor = new Color(0.7f, 0.75f, 0.75f, 1f);
            TNBaseStyle.UnitLabelStyle.border = new RectOffset(0, 0, 5, 5);
            TNBaseStyle.UnitLabelStyle.padding = new RectOffset(0, 0, 4, 4);
            TNBaseStyle.UnitLabelStyle.overflow = new RectOffset(0, 0, 0, 0);
            TNBaseStyle.UnitColorHex = ColorUtility.ToHtmlStringRGBA(UnitLabelStyle.normal.textColor);
        }
        private static void BuildFrames()
        {
            GUIStyle guiStyle = new GUIStyle(TNBaseStyle.Skin.window)
            {
                border = new RectOffset(25, 25, 35, 25),
                margin = new RectOffset(0, 0, 0, 0),
                padding = new RectOffset(10, 10, 44, 10),
                overflow = new RectOffset(0, 0, 0, 0),
                contentOffset = new Vector2(31f, -40f)
            };
            guiStyle.normal.background = AssetsLoader.LoadIcon("window");
            guiStyle.normal.background = AssetsLoader.LoadIcon("box");
            guiStyle.normal.textColor = Color.black;
            TNBaseStyle.SetAllFromNormal(guiStyle);
            guiStyle.alignment = TextAnchor.UpperLeft;
            guiStyle.stretchWidth = true;
            guiStyle.contentOffset = new Vector2(31f, -40f);
            TNBaseStyle.Skin.window = guiStyle;
            GUIStyle style1 = new GUIStyle(guiStyle);
            style1.normal.background = AssetsLoader.LoadIcon("Box");
            TNBaseStyle.SetAllFromNormal(style1);
            style1.border = new RectOffset(10, 10, 10, 10);
            style1.margin = new RectOffset(0, 0, 0, 0);
            style1.padding = new RectOffset(10, 10, 10, 10);
            style1.overflow = new RectOffset(0, 0, 0, 0);
            TNBaseStyle.Skin.box = style1;
            TNBaseStyle.Skin.scrollView = style1;
            GUIStyle style2 = new GUIStyle(GUI.skin.verticalScrollbar);
            style2.normal.background = AssetsLoader.LoadIcon("VerticalScroll");
            TNBaseStyle.SetAllFromNormal(style2);
            style2.border = new RectOffset(5, 5, 5, 5);
            style2.fixedWidth = 10f;
            TNBaseStyle.Skin.verticalScrollbar = style2;
            GUIStyle style3 = new GUIStyle(GUI.skin.verticalScrollbarThumb);
            style3.normal.background = AssetsLoader.LoadIcon("VerticalScroll_thumb");
            TNBaseStyle.SetAllFromNormal(style3);
            style3.border = new RectOffset(5, 5, 5, 5);
            style3.fixedWidth = 10f;
            TNBaseStyle.Skin.verticalScrollbarThumb = style3;
            TNBaseStyle.Separator = new GUIStyle(GUI.skin.box);
            TNBaseStyle.Separator.normal.background = AssetsLoader.LoadIcon("line");
            TNBaseStyle.Separator.border = new RectOffset(2, 2, 0, 0);
            TNBaseStyle.Separator.margin = new RectOffset(10, 10, 5, 5);
            TNBaseStyle.Separator.fixedHeight = 3f;
            TNBaseStyle.SetAllFromNormal(TNBaseStyle.Separator);
        }

        private static void BuildIcons()
        {
            TNBaseStyle.Gear = AssetsLoader.LoadIcon("Gear");
            TNBaseStyle.Icon = AssetsLoader.LoadIcon("Icon");
            TNBaseStyle.Cross = AssetsLoader.LoadIcon("Cross");
        }
        private static void BuildButtons()
        {
            TNBaseStyle.Button = new GUIStyle(GUI.skin.GetStyle("Button"));
            TNBaseStyle.Button.normal.background = AssetsLoader.LoadIcon("BigButton_Normal");
            TNBaseStyle.Button.normal.textColor = ColorTools.ParseColor("#FFFFFF");
            TNBaseStyle.SetAllFromNormal(TNBaseStyle.Button);
            TNBaseStyle.Button.hover.background = AssetsLoader.LoadIcon("BigButton_hover");
            TNBaseStyle.Button.active.background = AssetsLoader.LoadIcon("BigButton_hover");
            TNBaseStyle.Button.border = new RectOffset(5, 5, 5, 5);
            TNBaseStyle.Button.padding = new RectOffset(4, 4, 4, 4);
            TNBaseStyle.Button.overflow = new RectOffset(0, 0, 0, 0);
            TNBaseStyle.Button.alignment = TextAnchor.MiddleCenter;
            TNBaseStyle.Skin.button = TNBaseStyle.Button;
            TNBaseStyle.SmallButton = new GUIStyle(GUI.skin.GetStyle("Button"));
            TNBaseStyle.SmallButton.normal.background = AssetsLoader.LoadIcon("Small_Button");
            TNBaseStyle.SetAllFromNormal(TNBaseStyle.SmallButton);
            TNBaseStyle.SmallButton.hover.background = AssetsLoader.LoadIcon("Small_Button_hover");
            TNBaseStyle.SmallButton.active.background = AssetsLoader.LoadIcon("Small_Button_active");
            TNBaseStyle.SmallButton.onNormal = TNBaseStyle.SmallButton.active;
            TNBaseStyle.SetFromOn(TNBaseStyle.SmallButton);
            TNBaseStyle.SmallButton.border = new RectOffset(5, 5, 5, 5);
            TNBaseStyle.SmallButton.padding = new RectOffset(2, 2, 2, 2);
            TNBaseStyle.SmallButton.overflow = new RectOffset(0, 0, 0, 0);
            TNBaseStyle.SmallButton.alignment = TextAnchor.MiddleCenter;
            TNBaseStyle.BigButton = new GUIStyle(GUI.skin.GetStyle("Button"));
            TNBaseStyle.BigButton.normal.background = AssetsLoader.LoadIcon("BigButton_Normal");
            TNBaseStyle.BigButton.normal.textColor = ColorTools.ParseColor("#FFFFFF");
            TNBaseStyle.SetAllFromNormal(TNBaseStyle.BigButton);
            TNBaseStyle.BigButton.hover.background = AssetsLoader.LoadIcon("BigButton_Hover");
            TNBaseStyle.BigButton.active.background = AssetsLoader.LoadIcon("BigButton_Active");
            TNBaseStyle.BigButton.onNormal = TNBaseStyle.BigButton.active;
            TNBaseStyle.SetFromOn(TNBaseStyle.BigButton);
            TNBaseStyle.BigButton.border = new RectOffset(5, 5, 5, 5);
            TNBaseStyle.BigButton.padding = new RectOffset(8, 8, 10, 10);
            TNBaseStyle.BigButton.overflow = new RectOffset(0, 0, 0, 0);
            TNBaseStyle.BigButton.alignment = TextAnchor.MiddleCenter;
            TNBaseStyle.IconButton = new GUIStyle(TNBaseStyle.SmallButton)
            {
                padding = new RectOffset(4, 4, 4, 4)
            };
            TNBaseStyle.BigiconButton = new GUIStyle(TNBaseStyle.IconButton)
            {
                fixedWidth = 50f,
                fixedHeight = 50f,
                fontStyle = FontStyle.Bold
            };
            TNBaseStyle.CtrlButton = new GUIStyle(TNBaseStyle.SmallButton)
            {
                fixedHeight = 16f
            };
            TNBaseStyle.CtrlButton.normal.background = AssetsLoader.LoadIcon("Small_Button");
            TNBaseStyle.SetAllFromNormal(TNBaseStyle.CtrlButton);
            TNBaseStyle.CtrlButton.hover.background = AssetsLoader.LoadIcon("Small_Button_hover");
            TNBaseStyle.CtrlButton.active.background = AssetsLoader.LoadIcon("Small_Button_active");
            TNBaseStyle.CtrlButton.onNormal = TNBaseStyle.CtrlButton.active;
            TNBaseStyle.SetFromOn(TNBaseStyle.CtrlButton);
        }
        private static void BuildTabs()
        {
            TNBaseStyle.TabNormal = new GUIStyle(Button)
            {
                border = new RectOffset(5, 5, 5, 5),
                padding = new RectOffset(10, 10, 5, 5),
                overflow = new RectOffset(0, 0, 0, 0),
                alignment = TextAnchor.MiddleCenter,
                stretchWidth = true
            };
            TNBaseStyle.TabNormal.normal.background = AssetsLoader.LoadIcon("Tab_Normal");
            TNBaseStyle.SetAllFromNormal(TNBaseStyle.TabNormal);
            TNBaseStyle.TabNormal.hover.background = AssetsLoader.LoadIcon("Tab_Hover");
            TNBaseStyle.TabNormal.active.background = AssetsLoader.LoadIcon("Tab_Active");
            TNBaseStyle.TabNormal.onNormal = TNBaseStyle.TabNormal.active;
            TNBaseStyle.SetFromOn(TNBaseStyle.TabNormal);
            TNBaseStyle.TabActive = new GUIStyle(TNBaseStyle.TabNormal);
            TNBaseStyle.TabActive.normal.background = AssetsLoader.LoadIcon("Tab_On_normal");
            TNBaseStyle.SetAllFromNormal(TNBaseStyle.TabActive);
            TNBaseStyle.TabActive.hover.background = AssetsLoader.LoadIcon("Tab_On_hover");
            TNBaseStyle.TabActive.active.background = AssetsLoader.LoadIcon("Tab_On_Active");
            TNBaseStyle.TabActive.onNormal = TNBaseStyle.TabActive.active;
            TNBaseStyle.SetFromOn(TNBaseStyle.TabActive);
        }
        private static void BuildFoldout()
        {
            TNBaseStyle.FoldoutClose = new GUIStyle(TNBaseStyle.SmallButton)
            {
                fixedHeight = 30f,
                padding = new RectOffset(23, 2, 2, 2),
                border = new RectOffset(23, 7, 27, 3)
            };
            TNBaseStyle.FoldoutClose.normal.background = AssetsLoader.LoadIcon("Chapter_Off_Normal");
            TNBaseStyle.FoldoutClose.normal.textColor = ColorTools.ParseColor("#D4D4D4");
            TNBaseStyle.FoldoutClose.alignment = TextAnchor.MiddleLeft;
            TNBaseStyle.SetAllFromNormal(TNBaseStyle.FoldoutClose);
            TNBaseStyle.FoldoutClose.hover.background = AssetsLoader.LoadIcon("Chapter_Off_Hover");
            TNBaseStyle.FoldoutClose.active.background = AssetsLoader.LoadIcon("Chapter_Off_Active");
            TNBaseStyle.FoldoutClose = new GUIStyle(TNBaseStyle.FoldoutClose);
            TNBaseStyle.FoldoutClose.normal.background = AssetsLoader.LoadIcon("Chapter_On_Normal");
            TNBaseStyle.FoldoutClose.normal.textColor = ColorTools.ParseColor("#8BFF95");
            TNBaseStyle.SetAllFromNormal(TNBaseStyle.FoldoutClose);
            TNBaseStyle.FoldoutClose.hover.background = AssetsLoader.LoadIcon("Chapter_On_Hover");
            TNBaseStyle.FoldoutClose.active.background = AssetsLoader.LoadIcon("Chapter_On_Active");
        }
        private static void BuildToggle()
        {
            TNBaseStyle.Toggle = new GUIStyle(GUI.skin.GetStyle("Button"));
            TNBaseStyle.Toggle.normal.background = AssetsLoader.LoadIcon("Toggle_Off");
            TNBaseStyle.Toggle.normal.textColor = ColorTools.ParseColor("#C0C1E2");
            TNBaseStyle.SetAllFromNormal(TNBaseStyle.Toggle);
            TNBaseStyle.Toggle.onNormal.background = AssetsLoader.LoadIcon("Toggle_On");
            TNBaseStyle.Toggle.onNormal.textColor = ColorTools.ParseColor("#C0E2DC");
            TNBaseStyle.SetFromOn(TNBaseStyle.Toggle);
            TNBaseStyle.Toggle.fixedHeight = 32f;
            TNBaseStyle.Toggle.stretchWidth = false;
            TNBaseStyle.Toggle.border = new RectOffset(45, 5, 5, 5);
            TNBaseStyle.Toggle.padding = new RectOffset(34, 16, 0, 0);
            TNBaseStyle.Toggle.overflow = new RectOffset(0, 0, 0, 2);
            TNBaseStyle.ToggleError = new GUIStyle(TNBaseStyle.Toggle);
            TNBaseStyle.ToggleError.normal.textColor = Color.red;

        }
        private static void SetAllFromNormal(GUIStyle style)
        {
            style.hover = style.normal;
            style.active = style.normal;
            style.focused = style.normal;
            style.onNormal = style.normal;
            style.onHover = style.normal;
            style.onActive = style.normal;
            style.onFocused = style.normal;
        }
        private static void SetFromOn(GUIStyle style)
        {
            style.onHover = style.onNormal;
            style.onActive = style.onNormal;
            style.onFocused = style.onNormal;
        }
        private static GUISkin CopySkin(GUISkin source)
        {
            GUISkin newSkin = ScriptableObject.CreateInstance<GUISkin>();
            newSkin.font = source.font;
            newSkin.box = new GUIStyle(source.box);
            newSkin.label = new GUIStyle(source.label);
            newSkin.textField = new GUIStyle(source.textField);
            newSkin.textArea = new GUIStyle(source.textArea);
            newSkin.button = new GUIStyle(source.button);
            newSkin.toggle = new GUIStyle(source.toggle);
            newSkin.window = new GUIStyle(source.window);
            newSkin.horizontalScrollbar = new GUIStyle(source.horizontalScrollbar);
            newSkin.horizontalScrollbarThumb = new GUIStyle(source.horizontalScrollbarThumb);
            newSkin.horizontalScrollbarLeftButton = new GUIStyle(source.horizontalScrollbarLeftButton);
            newSkin.horizontalScrollbarRightButton = new GUIStyle(source.horizontalScrollbarRightButton);
            newSkin.verticalScrollbar = new GUIStyle(source.verticalScrollbar);
            newSkin.verticalScrollbarThumb = new GUIStyle(source.verticalScrollbarThumb);
            newSkin.verticalScrollbarUpButton = new GUIStyle(source.verticalScrollbarUpButton);
            newSkin.verticalScrollbarDownButton = new GUIStyle(source.verticalScrollbarDownButton);
            newSkin.scrollView = new GUIStyle(source.scrollView);

            return newSkin;
        }

    }
}