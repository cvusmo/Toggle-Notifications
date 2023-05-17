using SpaceWarp.API.UI;
using UnityEngine;


namespace ToggleNotifications.TNTools.UI
{
    public class TNBaseStyle
    {
        public static GUISkin Skin;
        private static bool _guiLoaded;
        public static GUIStyle Error;
        public static GUIStyle Warning;
        public static GUIStyle Label;
        public static GUIStyle MidText;
        public static GUIStyle ConsoleText;
        public static GUIStyle PhaseOk;
        public static GUIStyle PhaseWarning;
        public static GUIStyle PhaseError;
        public static GUIStyle IconsLabel;
        public static GUIStyle Title;
        public static GUIStyle SliderText;
        public static GUIStyle TextInputStyle;
        public static GUIStyle NameLabelStyle;
        public static GUIStyle ValueLabelStyle;
        public static GUIStyle UnitLabelStyle;
        public static string UnitColorHex;
        public static GUIStyle Separator;
        public static GUIStyle SliderLine;
        public static GUIStyle SliderNode;
        public static Texture2D Gear;
        public static Texture2D Icon;
        public static Texture2D TNIcon;
        public static Texture2D Cross;
        public static GUIStyle ProgressBarEmpty;
        public static GUIStyle ProgressBarFull;
        public static GUIStyle BigiconButton;
        public static GUIStyle IconButton;
        public static GUIStyle SmallButton;
        public static GUIStyle BigButton;
        public static GUIStyle Button;
        public static GUIStyle CtrlButton;
        public static GUIStyle TabNormal;
        public static GUIStyle TabActive;
        public static GUIStyle FoldoutClose;
        public static GUIStyle FoldoutOpen;
        public static GUIStyle Toggle;
        public static GUIStyle ToggleError;

        public static bool Init() => BuildStyles();

        public static bool BuildStyles()
        {
            if (_guiLoaded)
                return true;
            //Skin = CopySkin(Skins.ConsoleSkin);
            Skin = ScriptableObject.CreateInstance<GUISkin>();
            BuildFrames();
            BuildSliders();
            BuildButtons();
            BuildTabs();
            BuildFoldout();
            BuildToggle();
            BuildProgressBar();
            BuildIcons();
            BuildLabels();
            _guiLoaded = true;
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
            Error = new GUIStyle(GUI.skin.GetStyle("Label"));
            Warning = new GUIStyle(GUI.skin.GetStyle("Label"));
            Error.normal.textColor = Color.red;
            Warning.normal.textColor = Color.yellow;
            PhaseOk = new GUIStyle(GUI.skin.GetStyle("Label"));
            PhaseOk.normal.textColor = ColorTools.ParseColor("#00BC16");
            PhaseWarning = new GUIStyle(GUI.skin.GetStyle("Label"));
            PhaseWarning.normal.textColor = ColorTools.ParseColor("#BC9200");
            PhaseError = new GUIStyle(GUI.skin.GetStyle("Label"));
            PhaseError.normal.textColor = ColorTools.ParseColor("#B30F0F");
            ConsoleText = new GUIStyle(GUI.skin.GetStyle("Label"));
            ConsoleText.normal.textColor = ColorTools.ParseColor("#B6B8FA");
            ConsoleText.padding = new RectOffset(0, 0, 0, 0);
            ConsoleText.margin = new RectOffset(0, 0, 0, 0);
            SliderText = new GUIStyle(ConsoleText);
            SliderText.normal.textColor = ColorTools.ParseColor("#C0C1E2");
            MidText = new GUIStyle(SliderText);
            SliderText.margin = new RectOffset(5, 0, 0, 0);
            SliderText.contentOffset = new Vector2(8f, 5f);
            Label = new GUIStyle(GUI.skin.GetStyle("Label"))
            {
                margin = new RectOffset(0, 0, 0, 0),
                padding = new RectOffset(0, 0, 0, 0)
            };
            Title = new GUIStyle();
            Title.normal.textColor = ColorTools.ParseColor("#C0C1E2");
            TextInputStyle = new GUIStyle(GUI.skin.GetStyle("textField"))
            {
                alignment = TextAnchor.LowerCenter,
                padding = new RectOffset(10, 10, 0, 0),
                contentOffset = new Vector2(0.0f, 2f),
                fixedHeight = 18f,
                fixedWidth = 90f,
                clipping = TextClipping.Overflow,
                margin = new RectOffset(0, 0, 2, 0)
            };
            NameLabelStyle = new GUIStyle(GUI.skin.GetStyle("Label"));
            NameLabelStyle.border = new RectOffset(0, 0, 5, 5);
            NameLabelStyle.padding = new RectOffset(0, 0, 4, 4);
            NameLabelStyle.overflow = new RectOffset(0, 0, 0, 0);
            ValueLabelStyle = new GUIStyle(GUI.skin.GetStyle("Label"))
            {
                alignment = TextAnchor.MiddleRight
            };
            UnitLabelStyle = new GUIStyle(ValueLabelStyle)
            {
                fixedWidth = 24f,
                alignment = TextAnchor.MiddleLeft
            };
            UnitLabelStyle.normal.textColor = new Color(0.7f, 0.75f, 0.75f, 1f);
            UnitLabelStyle.border = new RectOffset(0, 0, 5, 5);
            UnitLabelStyle.padding = new RectOffset(0, 0, 4, 4);
            UnitLabelStyle.overflow = new RectOffset(0, 0, 0, 0);
            UnitColorHex = ColorUtility.ToHtmlStringRGBA(UnitLabelStyle.normal.textColor);
        }

        private static void BuildFrames()
        {
            GUIStyle guiStyle = new GUIStyle(Skin.window)
            {
                border = new RectOffset(25, 25, 35, 25),
                margin = new RectOffset(0, 0, 0, 0),
                padding = new RectOffset(10, 10, 44, 10),
                overflow = new RectOffset(0, 0, 0, 0),
                contentOffset = new Vector2(31f, -40f)
            };
            Debug.Log("Attempting window");
            //guiStyle.normal.background = AssetsLoader.LoadIcon("window");
            guiStyle.normal.background = AssetsLoader.LoadIcon("box");
            Debug.Log("Did it work?");
            guiStyle.normal.textColor = Color.black;
            SetAllFromNormal(guiStyle);
            guiStyle.alignment = TextAnchor.UpperLeft;
            guiStyle.stretchWidth = true;
            guiStyle.contentOffset = new Vector2(31f, -40f);
            Skin.window = guiStyle;
            GUIStyle style1 = new GUIStyle(guiStyle);
            style1.normal.background = AssetsLoader.LoadIcon("Box");
            SetAllFromNormal(style1);
            style1.border = new RectOffset(10, 10, 10, 10);
            style1.margin = new RectOffset(0, 0, 0, 0);
            style1.padding = new RectOffset(10, 10, 10, 10);
            style1.overflow = new RectOffset(0, 0, 0, 0);
            Skin.box = style1;
            Skin.scrollView = style1;
            GUIStyle style2 = new GUIStyle(GUI.skin.verticalScrollbar);
            style2.normal.background = AssetsLoader.LoadIcon("VerticalScroll");
            SetAllFromNormal(style2);
            style2.border = new RectOffset(5, 5, 5, 5);
            style2.fixedWidth = 10f;
            Skin.verticalScrollbar = style2;
            GUIStyle style3 = new GUIStyle(GUI.skin.verticalScrollbarThumb);
            style3.normal.background = AssetsLoader.LoadIcon("VerticalScroll_thumb");
            SetAllFromNormal(style3);
            style3.border = new RectOffset(5, 5, 5, 5);
            style3.fixedWidth = 10f;
            Skin.verticalScrollbarThumb = style3;
            Separator = new GUIStyle(GUI.skin.box);
            Separator.normal.background = AssetsLoader.LoadIcon("line");
            Separator.border = new RectOffset(2, 2, 0, 0);
            Separator.margin = new RectOffset(10, 10, 5, 5);
            Separator.fixedHeight = 3f;
            SetAllFromNormal(Separator);
        }

        private static void BuildSliders()
        {
            SliderLine = new GUIStyle(GUI.skin.horizontalSlider);
            SliderLine.normal.background = AssetsLoader.LoadIcon("Slider");
            SetAllFromNormal(SliderLine);
            SliderLine.border = new RectOffset(5, 5, 0, 0);
            SliderLine.border = new RectOffset(12, 14, 0, 0);
            SliderLine.fixedWidth = 0.0f;
            SliderLine.fixedHeight = 21f;
            SliderLine.margin = new RectOffset(0, 0, 2, 5);
            SliderNode = new GUIStyle(GUI.skin.horizontalSliderThumb);
            SliderNode.normal.background = AssetsLoader.LoadIcon("SliderNode");
            SetAllFromNormal(SliderNode);
            SliderNode.border = new RectOffset(0, 0, 0, 0);
            SliderNode.fixedWidth = 21f;
            SliderNode.fixedHeight = 21f;
        }

        private static void BuildIcons()
        {
            Gear = AssetsLoader.LoadIcon("Gear");
            Icon = AssetsLoader.LoadIcon("Icon");
            Cross = AssetsLoader.LoadIcon("Cross");
        }

        private static void BuildProgressBar()
        {
            ProgressBarEmpty = new GUIStyle(GUI.skin.box);
            ProgressBarEmpty.normal.background = AssetsLoader.LoadIcon("progress_empty");
            ProgressBarEmpty.border = new RectOffset(2, 2, 2, 2);
            ProgressBarEmpty.margin = new RectOffset(5, 5, 5, 5);
            ProgressBarEmpty.fixedHeight = 20f;
            SetAllFromNormal(ProgressBarEmpty);
            ProgressBarFull = new GUIStyle(ProgressBarEmpty);
            ProgressBarFull.normal.background = AssetsLoader.LoadIcon("progress_full");
            SetAllFromNormal(ProgressBarEmpty);
        }

        private static void BuildButtons()
        {
            Button = new GUIStyle(GUI.skin.GetStyle("Button"));
            Button.normal.background = AssetsLoader.LoadIcon("BigButton_Normal");
            Button.normal.textColor = ColorTools.ParseColor("#FFFFFF");
            SetAllFromNormal(Button);
            Button.hover.background = AssetsLoader.LoadIcon("BigButton_hover");
            Button.active.background = AssetsLoader.LoadIcon("BigButton_hover");
            Button.border = new RectOffset(5, 5, 5, 5);
            Button.padding = new RectOffset(4, 4, 4, 4);
            Button.overflow = new RectOffset(0, 0, 0, 0);
            Button.alignment = TextAnchor.MiddleCenter;
            Skin.button = Button;
            SmallButton = new GUIStyle(GUI.skin.GetStyle("Button"));
            SmallButton.normal.background = AssetsLoader.LoadIcon("Small_Button");
            SetAllFromNormal(SmallButton);
            SmallButton.hover.background = AssetsLoader.LoadIcon("Small_Button_hover");
            SmallButton.active.background = AssetsLoader.LoadIcon("Small_Button_active");
            SmallButton.onNormal = SmallButton.active;
            SetFromOn(SmallButton);
            SmallButton.border = new RectOffset(5, 5, 5, 5);
            SmallButton.padding = new RectOffset(2, 2, 2, 2);
            SmallButton.overflow = new RectOffset(0, 0, 0, 0);
            SmallButton.alignment = TextAnchor.MiddleCenter;
            BigButton = new GUIStyle(GUI.skin.GetStyle("Button"));
            BigButton.normal.background = AssetsLoader.LoadIcon("BigButton_Normal");
            BigButton.normal.textColor = ColorTools.ParseColor("#FFFFFF");
            SetAllFromNormal(BigButton);
            BigButton.hover.background = AssetsLoader.LoadIcon("BigButton_Hover");
            BigButton.active.background = AssetsLoader.LoadIcon("BigButton_Active");
            BigButton.onNormal = BigButton.active;
            SetFromOn(BigButton);
            BigButton.border = new RectOffset(5, 5, 5, 5);
            BigButton.padding = new RectOffset(8, 8, 10, 10);
            BigButton.overflow = new RectOffset(0, 0, 0, 0);
            BigButton.alignment = TextAnchor.MiddleCenter;
            IconButton = new GUIStyle(SmallButton)
            {
                padding = new RectOffset(4, 4, 4, 4)
            };
            BigiconButton = new GUIStyle(IconButton)
            {
                fixedWidth = 50f,
                fixedHeight = 50f,
                fontStyle = FontStyle.Bold
            };
            CtrlButton = new GUIStyle(SmallButton)
            {
                fixedHeight = 16f
            };
            CtrlButton.normal.background = AssetsLoader.LoadIcon("Small_Button");
            SetAllFromNormal(CtrlButton);
            CtrlButton.hover.background = AssetsLoader.LoadIcon("Small_Button_hover");
            CtrlButton.active.background = AssetsLoader.LoadIcon("Small_Button_active");
            CtrlButton.onNormal = CtrlButton.active;
            SetFromOn(CtrlButton);
        }

        private static void BuildTabs()
        {
            TabNormal = new GUIStyle(Button)
            {
                border = new RectOffset(5, 5, 5, 5),
                padding = new RectOffset(10, 10, 5, 5),
                overflow = new RectOffset(0, 0, 0, 0),
                alignment = TextAnchor.MiddleCenter,
                stretchWidth = true
            };
            TabNormal.normal.background = AssetsLoader.LoadIcon("Tab_Normal");
            SetAllFromNormal(TabNormal);
            TabNormal.hover.background = AssetsLoader.LoadIcon("Tab_Hover");
            TabNormal.active.background = AssetsLoader.LoadIcon("Tab_Active");
            TabNormal.onNormal = TabNormal.active;
            SetFromOn(TabNormal);
            TabActive = new GUIStyle(TabNormal);
            TabActive.normal.background = AssetsLoader.LoadIcon("Tab_On_normal");
            SetAllFromNormal(TabActive);
            TabActive.hover.background = AssetsLoader.LoadIcon("Tab_On_hover");
            TabActive.active.background = AssetsLoader.LoadIcon("Tab_On_Active");
            TabActive.onNormal = TabActive.active;
            SetFromOn(TabActive);
        }

        private static void BuildFoldout()
        {
            FoldoutClose = new GUIStyle(SmallButton)
            {
                fixedHeight = 30f,
                padding = new RectOffset(23, 2, 2, 2),
                border = new RectOffset(23, 7, 27, 3)
            };
            FoldoutClose.normal.background = AssetsLoader.LoadIcon("Chapter_Off_Normal");
            FoldoutClose.normal.textColor = ColorTools.ParseColor("#D4D4D4");
            FoldoutClose.alignment = TextAnchor.MiddleLeft;
            SetAllFromNormal(FoldoutClose);
            FoldoutClose.hover.background = AssetsLoader.LoadIcon("Chapter_Off_Hover");
            FoldoutClose.active.background = AssetsLoader.LoadIcon("Chapter_Off_Active");
            FoldoutOpen = new GUIStyle(FoldoutClose);
            FoldoutOpen.normal.background = AssetsLoader.LoadIcon("Chapter_On_Normal");
            FoldoutOpen.normal.textColor = ColorTools.ParseColor("#8BFF95");
            SetAllFromNormal(FoldoutOpen);
            FoldoutOpen.hover.background = AssetsLoader.LoadIcon("Chapter_On_Hover");
            FoldoutOpen.active.background = AssetsLoader.LoadIcon("Chapter_On_Active");
        }

        private static void BuildToggle()
        {
            Toggle = new GUIStyle(GUI.skin.GetStyle("Button"));
            Toggle.normal.background = AssetsLoader.LoadIcon("Toggle_Off");
            Toggle.normal.textColor = ColorTools.ParseColor("#C0C1E2");
            SetAllFromNormal(Toggle);
            Toggle.onNormal.background = AssetsLoader.LoadIcon("Toggle_On");
            Toggle.onNormal.textColor = ColorTools.ParseColor("#C0E2DC");
            SetFromOn(Toggle);
            Toggle.fixedHeight = 32f;
            Toggle.stretchWidth = false;
            Toggle.border = new RectOffset(45, 5, 5, 5);
            Toggle.padding = new RectOffset(34, 16, 0, 0);
            Toggle.overflow = new RectOffset(0, 0, 0, 2);
            ToggleError = new GUIStyle(Toggle);
            ToggleError.normal.textColor = Color.red;
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

        private static GUISkin CopySkin(GUISkin source) => new GUISkin()
        {
            font = source.font,
            box = new GUIStyle(source.box),
            label = new GUIStyle(source.label),
            textField = new GUIStyle(source.textField),
            textArea = new GUIStyle(source.textArea),
            button = new GUIStyle(source.button),
            toggle = new GUIStyle(source.toggle),
            window = new GUIStyle(source.window),
            horizontalSlider = new GUIStyle(source.horizontalSlider),
            horizontalSliderThumb = new GUIStyle(source.horizontalSliderThumb),
            verticalSlider = new GUIStyle(source.verticalSlider),
            verticalSliderThumb = new GUIStyle(source.verticalSliderThumb),
            horizontalScrollbar = new GUIStyle(source.horizontalScrollbar),
            horizontalScrollbarThumb = new GUIStyle(source.horizontalScrollbarThumb),
            horizontalScrollbarLeftButton = new GUIStyle(source.horizontalScrollbarLeftButton),
            horizontalScrollbarRightButton = new GUIStyle(source.horizontalScrollbarRightButton),
            verticalScrollbar = new GUIStyle(source.verticalScrollbar),
            verticalScrollbarThumb = new GUIStyle(source.verticalScrollbarThumb),
            verticalScrollbarUpButton = new GUIStyle(source.verticalScrollbarUpButton),
            verticalScrollbarDownButton = new GUIStyle(source.verticalScrollbarDownButton),
            scrollView = new GUIStyle(source.scrollView)
        };
    }
}