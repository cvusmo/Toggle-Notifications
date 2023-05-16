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
            if (!ToolTipsManager.Show || (double)Time.time <= (double)ToolTipsManager.ShowTime)
                return;

            float maxWidth;
            GUI.skin.button.CalcMinMaxWidth(new GUIContent(DrawToolTip), out float minWidth, out maxWidth);
            Rect windowFrame = new Rect(Input.mousePosition.x + Offset.x, Screen.height - Input.mousePosition.y + Offset.y, maxWidth, 10f);
            WindowTool.CheckWindowPos(ref windowFrame);

            GUI.Window(3, windowFrame, WindowFunction, "", GUI.skin.button);
        }
        private static void WindowFunction(int windowID) => GUILayout.Label(ToolTipsManager.DrawToolTip);
    }
}