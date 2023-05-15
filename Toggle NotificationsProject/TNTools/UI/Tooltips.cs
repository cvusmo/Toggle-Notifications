using ToggleNotifications.TNTools.UI;
using UnityEngine;

namespace ToggleNotifications.TNTools.UI
{
    public class ToolTipsManager
    {
        private static float ShowTime;
        private const float DELAY = 0.5f;
        private static bool Show = false;
        private static Vector2 Offset = new Vector2(20f, 10f);
        private static string LastToolTip;
        private static string DrawToolTip;

        public static void SetToolTip(string tooltip)
        {
            if (Event.current.type != EventType.Repaint)
                return;
            if (ToolTipsManager.LastToolTip != tooltip)
            {
                if (!string.IsNullOrEmpty(tooltip))
                {
                    ToolTipsManager.Show = true;
                    ToolTipsManager.ShowTime = Time.time + 0.5f;
                    ToolTipsManager.DrawToolTip = tooltip;
                }
                else
                    ToolTipsManager.Show = false;
            }
            ToolTipsManager.LastToolTip = tooltip;
        }

        public static void DrawToolTips()
        {
            if (!Show)
                return;

            if (Time.time > ShowTime)
            {
                GUI.skin.button.CalcMinMaxWidth(new GUIContent(DrawToolTip), out float _minWidth, out float _maxWidth);
                Rect _tooltipPos = new Rect(Input.mousePosition.x + Offset.x, Screen.height - Input.mousePosition.y + Offset.y, _maxWidth, 10);
                WindowTool.CheckWindowPos(ref _tooltipPos);

                GUILayout.Window(3, _tooltipPos, WindowFunction, "", GUI.skin.button);
            }
        }

        static void WindowFunction(int windowID)
        {
            //Debug.Log(DrawToolTip);
            GUILayout.Label(DrawToolTip);
        }
    }
}