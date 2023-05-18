using System.Globalization;
using UnityEngine;

namespace ToggleNotifications.TNTools.UI
{
    public class UITools
    {
        public static int GetEnumValue<T>(T inputEnum) where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("Input type must be an enum.");
            return inputEnum.ToInt32((IFormatProvider)CultureInfo.InvariantCulture.NumberFormat);
        }

        public static TEnum EnumGrid<TEnum>(string label, TEnum value, string[] labels) where TEnum : struct, Enum
        {
            int hashCode = value.GetHashCode();
            UITools.Label(label);
            string[] texts = labels;
            int length = labels.Length;
            GUILayoutOption[] guiLayoutOptionArray = Array.Empty<GUILayoutOption>();
            return (TEnum)Enum.ToObject(typeof(TEnum), GUILayout.SelectionGrid(hashCode, texts, length, guiLayoutOptionArray));
        }
        public static bool Toggle(bool isOn, string txt, string tooltip = null) => tooltip != null ? GUILayout.Toggle(isOn, new GUIContent(txt, tooltip), TNBaseStyle.Toggle) : GUILayout.Toggle(isOn, txt, TNBaseStyle.Toggle);

        public static bool BigToggleButton(bool isOn, string txtRun, string txtStop)
        {
            int minWidth = 150;
            string text = isOn ? txtStop : txtRun;
            isOn = GUILayout.Toggle((isOn ? 1 : 0) != 0, text, TNBaseStyle.BigButton, GUILayout.MinWidth((float)minWidth));
            return isOn;
        }

        public static bool SmallToggleButton(
          bool isOn,
          string txtRun,
          string txtStop,
          int widthOverride = 0)
        {
            int minWidth = widthOverride > 0 ? widthOverride : 150;
            string text = isOn ? txtStop : txtRun;
            isOn = GUILayout.Toggle((isOn ? 1 : 0) != 0, text, TNBaseStyle.SmallButton, GUILayout.MinWidth((float)minWidth));
            return isOn;
        }

        public static bool BigButton(string txt)
        {
            int minWidth = 150;
            return GUILayout.Button(txt, TNBaseStyle.BigButton, GUILayout.MinWidth((float)minWidth));
        }

        public static bool SmallButton(string txt) => GUILayout.Button(txt, TNBaseStyle.SmallButton);

        public static bool CtrlButton(string txt) => GUILayout.Button(txt, TNBaseStyle.CtrlButton);

        public static bool BigIconButton(string txt) => GUILayout.Button(txt, TNBaseStyle.BigiconButton);

        public static bool ListButton(string txt) => GUILayout.Button(txt, TNBaseStyle.Button, GUILayout.ExpandWidth(true));

        public static bool miniToggle(bool value, string txt, string tooltip) => GUILayout.Toggle((value ? 1 : 0) != 0, new GUIContent(txt, tooltip), TNBaseStyle.SmallButton, GUILayout.Height(20f));

        public static bool miniButton(string txt, string tooltip = "") => GUILayout.Button(new GUIContent(txt, tooltip), TNBaseStyle.SmallButton, GUILayout.Height(20f));

        public static bool ToolTipButton(string tooltip) => GUILayout.Button(new GUIContent("?", tooltip), TNBaseStyle.SmallButton, GUILayout.Width(16f), GUILayout.Height(20f));

        public static bool BigIconButton(Texture2D icon) => GUILayout.Button((Texture)icon, TNBaseStyle.BigiconButton);

        public static void Title(string txt) => GUILayout.Label("<b>" + txt + "</b>", TNBaseStyle.Title);

        public static void Label(string txt, GUIStyle thisStyle = null)
        {
            if (thisStyle == null)
                GUILayout.Label(txt, TNBaseStyle.Label);
            else
                GUILayout.Label(txt, thisStyle);
        }

        public static void OK(string txt) => GUILayout.Label(txt, TNBaseStyle.PhaseOk);

        public static void Warning(string txt) => GUILayout.Label(txt, TNBaseStyle.PhaseWarning);

        public static void Error(string txt) => GUILayout.Label(txt, TNBaseStyle.PhaseError);

        public static void Console(string txt) => GUILayout.Label(txt, TNBaseStyle.ConsoleText);

        public static void Mid(string txt) => GUILayout.Label(txt, TNBaseStyle.MidText);
        public static void Separator() => GUILayout.Box("", TNBaseStyle.Separator);

        public static void ProgressBar(double value, double min, double max) => UITools.ProgressBar((float)value, (float)min, (float)max);

        public static void ProgressBar(float value, float min, float max)
        {
            float num = Mathf.InverseLerp(min, max, value);
            GUILayout.Box("", TNBaseStyle.ProgressBarEmpty, GUILayout.ExpandWidth(true));
            Rect lastRect = GUILayoutUtility.GetLastRect();
            lastRect.width = Mathf.Clamp(lastRect.width * num, 4f, 1E+07f);
            GUI.Box(lastRect, "", TNBaseStyle.ProgressBarFull);
        }

        public static void Right_Left_Text(string right_txt, string left_txt)
        {
            GUILayout.BeginHorizontal();
            UITools.Mid(right_txt);
            GUILayout.FlexibleSpace();
            UITools.Mid(left_txt);
            GUILayout.EndHorizontal();
            GUILayout.Space(10f);
        }

        public static Vector2 BeginScrollView(Vector2 scrollPos, int height) => GUILayout.BeginScrollView(scrollPos, false, true, GUILayout.MinWidth(250f), GUILayout.Height((float)height));
    }
}
