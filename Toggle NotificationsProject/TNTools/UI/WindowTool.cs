using UnityEngine;

namespace ToggleNotifications.TNTools.UI
{
    public class WindowTool
    {
        public static void CheckWindowPos(ref Rect windowFrame)
        {
            if ((double)windowFrame.xMax > (double)Screen.width)
            {
                float num = (float)Screen.width - windowFrame.xMax;
                windowFrame.x += num;
            }
            if ((double)windowFrame.yMax > (double)Screen.height)
            {
                float num = (float)Screen.height - windowFrame.yMax;
                windowFrame.y += num;
            }
            if ((double)windowFrame.xMin < 0.0)
                windowFrame.x = 0.0f;
            if ((double)windowFrame.yMin >= 0.0)
                return;
            windowFrame.y = 0.0f;
        }

        public static void CheckMainWindowPos(ref Rect windowFrame, int windowWidth)
        {
            if (windowFrame == Rect.zero)
            {
                int x = TNBaseSettings.WindowXPos;
                int y = TNBaseSettings.WindowYPos;
                if (x == -1)
                {
                    x = 100;
                    y = 50;
                }
                windowFrame = new Rect((float)x, (float)y, windowWidth, 100f);
            }
            CheckWindowPos(ref windowFrame);
        }
    }
}
