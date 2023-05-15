using System;

namespace ToggleNotifications.TNTools.UI
{
    public class StrTool
    {
        public const double AstronomicalUnit = 149597870700.0;
        public static string DurationToString(double secs)
        {
            string str = "";
            if (secs < 0.0)
            {
                secs = -secs;
                str = "- ";
            }
            if (secs > 21600.0)
            {
                int num = (int)(secs / 21600.0);
                secs -= (double)(num * 21600);
                str += string.Format("{0}d ", (object)num);
            }
            try
            {
                TimeSpan timeSpan = TimeSpan.FromSeconds(secs);
                return str + string.Format("{0:D2}:{1:D2}:{2:D2}:{3:D3}", (object)timeSpan.Hours, (object)timeSpan.Minutes, (object)timeSpan.Seconds, (object)timeSpan.Milliseconds);
            }
            catch (Exception ex)
            {
                return str + string.Format("{0:n2} s ({1})", (object)secs, ex.Message);
            }
        }
        public static double Parsec { get; } = 3.08567758149137E+16;
        public static string DistanceToString(double meters)
        {
            string str = "";
            if (meters < 0.0)
            {
                str = "-";
                meters = -meters;
            }
            if (meters > StrTool.Parsec / 10.0)
                return string.Format("{0}{1:n2} pc", (object)str, (object)(meters / StrTool.Parsec));
            if (meters > 14959787070.0)
                return string.Format("{0}{1:n2} AU", (object)str, (object)(meters / 149597870700.0));
            if (meters > 997.0)
                return string.Format("{0}{1:n2} km", (object)str, (object)(meters / 1000.0));
            return meters < 1.0 ? string.Format("{0}{1:0} cm", (object)str, (object)(meters * 100.0)) : str + meters.ToString("0") + " m";
        }
        public static string VectorToString(Vector3d vec) => string.Format("{0:n2} {1:n2} {2:n2}", (object)vec.x, (object)vec.y, (object)vec.z);
    }
}
