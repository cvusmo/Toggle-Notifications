using UnityEngine;

namespace ToggleNotifications.TNTools.UI;

public class WindowTool
{
    public static void CheckWindowPos(ref Rect windowFrame)
    {
        if (windowFrame.xMax > Screen.width)
        {
            float dx = Screen.width - windowFrame.xMax;
            windowFrame.x += dx;
        }
        if (windowFrame.yMax > Screen.height)
        {
            float dy = Screen.height - windowFrame.yMax;
            windowFrame.y += dy;
        }
        if (windowFrame.xMin < 0)
        {
            windowFrame.x = 0;
        }
        if (windowFrame.yMin < 0)
        {
            windowFrame.y = 0;
        }
    }
    public static void CheckMainWindowPos(ref Rect windowFrame)
    {
        if (windowFrame == Rect.zero)
        {
            int windowXPos = TNBaseSettings.WindowXPos;
            int xPos = windowXPos;
            int yPos = TNBaseSettings.WindowYPos;

            if (xPos == -1)
            {
                xPos = 100;
                yPos = 50;
            }

            windowFrame = new Rect(xPos, yPos, 500, 100);
        }

        CheckWindowPos(ref windowFrame);
    }
}