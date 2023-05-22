﻿using BepInEx.Logging;
using KSP.Game;
using System.Text.RegularExpressions;
using UnityEngine;

namespace ToggleNotifications.TNTools.UI
{
    internal class UIFields
    {
        internal static Dictionary<string, string> TempDict = new Dictionary<string, string>();
        internal static List<string> InputFields = new List<string>();
        private static bool InputState = true;
        internal static ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource("KTools.UIFields");

        internal static bool GameInputState
        {
            get => UIFields.InputState;
            set
            {
                if (UIFields.InputState != value)
                {
                    UIFields.Logger.LogWarning((object)"input mode changed");
                    if (value)
                        GameManager.Instance.Game.Input.Enable();
                    else
                        GameManager.Instance.Game.Input.Disable();
                }
                UIFields.InputState = value;
            }
        }

        internal static void CheckEditor() => UIFields.GameInputState = !UIFields.InputFields.Contains(GUI.GetNameOfFocusedControl());

        internal static double DoubleField(
          string entryName,
          double value,
          GUIStyle thisStyle = null,
          bool parseAsTime = false)
        {
            string format = "HH:mm:ss";
            string str1 = !UIFields.TempDict.ContainsKey(entryName) ? (!parseAsTime ? value.ToString() : value.ToString(format)) : UIFields.TempDict[entryName];
            if (!UIFields.InputFields.Contains(entryName))
                UIFields.InputFields.Add(entryName);
            Color color = GUI.color;
            bool flag;
            double result1;
            if (parseAsTime)
            {
                if (str1 == format || str1.Length < 1)
                {
                    flag = true;
                    result1 = 0.0;
                }
                else
                {
                    TimeSpan result2;
                    flag = TimeSpan.TryParse(str1, out result2);
                    result1 = result2.TotalSeconds;
                }
            }
            else
                flag = double.TryParse(str1, out result1);
            if (!flag)
                GUI.color = Color.red;
            GUI.SetNextControlName(entryName);
            string str2;
            if (thisStyle != null)
                str2 = GUILayout.TextField(str1, thisStyle, GUILayout.Width(90f));
            else
                str2 = GUILayout.TextField(str1, GUILayout.Width(90f));
            GUI.color = color;
            UIFields.TempDict[entryName] = str2;
            return flag ? result1 : value;
        }

        internal static int IntField(
          string entryName,
          string label,
          int value,
          int min,
          int max,
          string tooltip = "")
        {
            string text = value.ToString();
            if (UIFields.TempDict.ContainsKey(entryName))
                text = UIFields.TempDict[entryName];
            if (!UIFields.InputFields.Contains(entryName))
                UIFields.InputFields.Add(entryName);
            GUILayout.BeginHorizontal();
            if (!string.IsNullOrEmpty(label))
                GUILayout.Label(label);
            GUI.SetNextControlName(entryName);
            string s = Regex.Replace(GUILayout.TextField(text, GUILayout.Width(100f)), "[^\\d-]+", "");
            UIFields.TempDict[entryName] = s;
            bool flag = true;
            int result;
            if (!int.TryParse(s, out result))
                flag = false;
            if (result < min)
            {
                flag = false;
                result = value;
            }
            else if (result > max)
            {
                flag = false;
                result = value;
            }
            if (!flag)
                GUILayout.Label("!!!", GUILayout.Width(30f));
            if (!string.IsNullOrEmpty(tooltip))
                UITools.ToolTipButton(tooltip);
            GUILayout.EndHorizontal();
            return result;
        }
    }
}