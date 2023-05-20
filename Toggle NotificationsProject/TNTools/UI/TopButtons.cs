using UnityEngine;

namespace ToggleNotifications.TNTools.UI
{
    internal class TopButtons
    {
        private static Rect Position = Rect.zero;
        private const int SPACE = 25;

        internal static void Init(float widthWindow)
        {
            TopButtons.Position = new Rect(widthWindow - 5f, 4f, 23f, 23f);
        }

        internal static void SetPosition(Rect position)
        {
            TopButtons.Position = position;
        }

        internal static bool Button(string txt)
        {
            TopButtons.Position.x -= 25f;
            return GUI.Button(TopButtons.Position, txt, TNBaseStyle.SmallButton);
        }

        internal static bool Button(Rect position, string txt)
        {
            return GUI.Button(position, txt, TNBaseStyle.SmallButton);
        }

        internal static bool Button(Texture2D icon)
        {
            TopButtons.Position.x -= 25f;
            return GUI.Button(TopButtons.Position, (Texture)icon, TNBaseStyle.IconButton);
        }

        internal static bool Button(Rect position, Texture2D icon)
        {
            return GUI.Button(position, (Texture)icon, TNBaseStyle.IconButton);
        }

        internal static bool Toggle(bool value, string txt)
        {
            TopButtons.Position.x -= 25f;
            return GUI.Toggle(TopButtons.Position, value, txt, TNBaseStyle.SmallButton);
        }

        internal static bool Toggle(Rect position, bool value, string txt)
        {
            return GUI.Toggle(position, value, txt, TNBaseStyle.SmallButton);
        }

        internal static bool Toggle(bool value, Texture2D icon)
        {
            TopButtons.Position.x -= 25f;
            return GUI.Toggle(TopButtons.Position, value, (Texture)icon, TNBaseStyle.IconButton);
        }

        internal static bool Toggle(Rect position, bool value, Texture2D icon)
        {
            return GUI.Toggle(position, value, (Texture)icon, TNBaseStyle.IconButton);
        }
    }
}
