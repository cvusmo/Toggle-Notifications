using System.Globalization;
using UnityEngine;

namespace ToggleNotifications.TNTools.UI
{
    public class ColorTools
    {
        public static void ToHSV(Color col, out float h, out float s, out float v)
        {
            float r = col.r;
            float g = col.g;
            float b = col.b;
            float num1 = Mathf.Min(r, Mathf.Min(g, b));
            float num2 = Mathf.Max(r, Mathf.Max(g, b));
            v = num2;
            float num3 = num2 - num1;
            if ((double)num3 != 0.0)
            {
                s = num3 / num2;
                h = (double)r != (double)num2 ? (double)g != (double)num2 ? (float)(4.0 + ((double)r - (double)g) / (double)num3) : (float)(2.0 + ((double)b - (double)r) / (double)num3) : (g - b) / num3;
                h *= 0.166666672f;
                if ((double)h >= 0.0)
                    return;
                ++h;
            }
            else
            {
                s = 0.0f;
                h = 0.0f;
            }
        }

        public static Color FromHSV(float hue, float saturation, float value, float alpha)
        {
            hue *= 360f;
            int num1 = (int)Mathf.Floor(hue / 60f) % 6;
            float num2 = hue / 60f - Mathf.Floor(hue / 60f);
            float num3 = value;
            float num4 = value * (1f - saturation);
            float num5 = value * (float)(1.0 - (double)num2 * (double)saturation);
            float num6 = value * (float)(1.0 - (1.0 - (double)num2) * (double)saturation);
            switch (num1)
            {
                case 0:
                    return new Color(num3, num6, num4, alpha);
                case 1:
                    return new Color(num5, num3, num4, alpha);
                case 2:
                    return new Color(num4, num3, num6, alpha);
                case 3:
                    return new Color(num4, num5, num3, alpha);
                case 4:
                    return new Color(num6, num4, num3, alpha);
                default:
                    return new Color(num3, num4, num5, alpha);
            }
        }

        public static Color ParseColor(string color)
        {
            if (color == "")
                return Color.white;
            color = color.ToLower();
            switch (color)
            {
                case "black":
                    return Color.black;
                case "blue":
                    return Color.blue;
                case "clear":
                    return Color.clear;
                case "cyan":
                    return Color.cyan;
                case "gray":
                    return Color.gray;
                case "green":
                    return Color.green;
                case "grey":
                    return Color.grey;
                case "magenta":
                    return Color.magenta;
                case "orange":
                    return new Color(1f, 0.76f, 0.0f);
                case "red":
                    return Color.red;
                case "white":
                    return Color.white;
                case "yellow":
                    return Color.yellow;
                default:
                    if (color.StartsWith("#"))
                        color = color.Substring(1);
                    while (color.Length < 6)
                        color += "0";
                    int result1;
                    int.TryParse(color.Substring(0, 2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture.NumberFormat, out result1);
                    int result2;
                    int.TryParse(color.Substring(2, 2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture.NumberFormat, out result2);
                    int result3;
                    int.TryParse(color.Substring(4, 2), NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture.NumberFormat, out result3);
                    return new Color(result1 / (float)byte.MaxValue, result2 / (float)byte.MaxValue, result3 / (float)byte.MaxValue);
            }
        }

        public static string FormatColorHtml(Color col) => string.Format("{0:X2}{1:X2}{2:X2}", (int)(col.r * (double)byte.MaxValue), (int)(col.g * (double)byte.MaxValue), (int)(col.b * (double)byte.MaxValue));

        public static Color[] GetRandomColorArray(int Nb, float saturation = 1f)
        {
            float num = 1f / Nb;
            Color[] randomColorArray = new Color[Nb];
            float hue = UnityEngine.Random.Range(0.0f, 1f);
            for (int index = 0; index < Nb; ++index)
            {
                randomColorArray[index] = FromHSV(hue, saturation, 1f, 1f);
                hue += num;
            }
            return randomColorArray;
        }

        public static Color[] GetRainbowColorArray(int Nb)
        {
            float num = 1f / Nb;
            Color[] rainbowColorArray = new Color[Nb];
            float hue = 0.0f;
            for (int index = 0; index < Nb; ++index)
            {
                rainbowColorArray[index] = FromHSV(hue, 1f, 1f, 1f);
                hue += num;
            }
            return rainbowColorArray;
        }

        public static Color RandomColor() => FromHSV(UnityEngine.Random.Range(0.0f, 1f), 1f, 1f, 1f);

        public static Color ChangeColorHSV(Color source, float deltaH, float deltaS, float deltaV)
        {
            float h;
            float s;
            float v;
            ToHSV(source, out h, out s, out v);
            return FromHSV(h + deltaH, s + deltaS, v + deltaV, source.a);
        }
    }
}