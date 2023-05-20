using UnityEngine;

namespace ToggleNotifications.TNTools.UI
{
    public class TopButtons
    {
        private static Rect Position = Rect.zero;
        private const int SPACE = 25;

        public static void Init(float widthWindow)
        {
            TopButtons.Position = new Rect(widthWindow - 5f, 4f, 23f, 23f);
        }

        public static void SetPosition(Rect position)
        {
            TopButtons.Position = position;
        }

        public static bool Button(string txt)
        {
            TopButtons.Position.x -= 25f;
            return GUI.Button(TopButtons.Position, txt, TNBaseStyle.SmallButton);
        }

        public static bool Button(Rect position, string txt)
        {
            return GUI.Button(position, txt, TNBaseStyle.SmallButton);
        }

        public static bool Button(Texture2D icon)
        {
            TopButtons.Position.x -= 25f;
            return GUI.Button(TopButtons.Position, (Texture)icon, TNBaseStyle.IconButton);
        }

        public static bool Button(Rect position, Texture2D icon)
        {
            return GUI.Button(position, (Texture)icon, TNBaseStyle.IconButton);
        }

        public static bool Toggle(bool value, string txt)
        {
            TopButtons.Position.x -= 25f;
            return GUI.Toggle(TopButtons.Position, value, txt, TNBaseStyle.SmallButton);
        }

        public static bool Toggle(Rect position, bool value, string txt)
        {
            return GUI.Toggle(position, value, txt, TNBaseStyle.SmallButton);
        }

        public static bool Toggle(bool value, Texture2D icon)
        {
            TopButtons.Position.x -= 25f;
            return GUI.Toggle(TopButtons.Position, value, (Texture)icon, TNBaseStyle.IconButton);
        }

        public static bool Toggle(Rect position, bool value, Texture2D icon)
        {
            return GUI.Toggle(position, value, (Texture)icon, TNBaseStyle.IconButton);
        }
    }
}
