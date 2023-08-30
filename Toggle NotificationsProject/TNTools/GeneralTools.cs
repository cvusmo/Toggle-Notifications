using KSP.Game;
using KSP.Sim.Maneuver;
using System;
using System.Text.RegularExpressions;

namespace ToggleNotifications.TNTools
{
    public static class GeneralTools
    {
        public static GameInstance Game => (UnityEngine.Object)GameManager.Instance == (UnityEngine.Object)null ? (GameInstance)null : GameManager.Instance.Game;

        public static double Current_UT => GeneralTools.Game.UniverseModel.UniverseTime;

        public static double GetNumberString(string str)
        {
            string s = Regex.Replace(str, "[^0-9.]", "");
            return s.Length <= 0 ? -1.0 : double.Parse(s);
        }

        public static int ClampInt(int value, int min, int max) => value < min ? min : (value <= max ? value : max);

        public static Vector3d CorrectEuler(Vector3d euler)
        {
            Vector3d vector3d = euler;
            if (vector3d.x > 180.0)
                vector3d.x -= 360.0;
            if (vector3d.y > 180.0)
                vector3d.y -= 360.0;
            if (vector3d.z > 180.0)
                vector3d.z -= 360.0;
            return vector3d;
        }

        public static double RemainingStartTime(ManeuverNodeData node) => node.Time - GeneralTools.Game.UniverseModel.UniverseTime;

        public static Guid CreateGuid() => Guid.NewGuid();
    }
}
