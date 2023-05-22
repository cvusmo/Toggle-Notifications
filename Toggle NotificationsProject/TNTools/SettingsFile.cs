
using BepInEx.Logging;
using Newtonsoft.Json;
using System.Globalization;
using UnityEngine;

namespace ToggleNotifications.TNTools
{
    internal class SettingsFile
    {
        protected string FilePath = "";
        Dictionary<string, string> Data = new();

        internal SettingsFile(string file_path)
        {
            this.FilePath = file_path;
            Load();
        }

        internal ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource("K2D2.SettingsFile");

        protected void Load()
        {

            CultureInfo _previousCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            try
            {
                this.Data = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(FilePath));
            }
            catch (System.Exception)
            {
                Logger.LogWarning($"Error loading {FilePath}");
            }

            Thread.CurrentThread.CurrentCulture = _previousCulture;
        }

        protected void Save()
        {
            CultureInfo _previousCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
            try
            {
                File.WriteAllText(FilePath, JsonConvert.SerializeObject(Data));
            }
            catch (System.Exception)
            {
                Logger.LogError($"Error saving {this.FilePath}");
            }

            Thread.CurrentThread.CurrentCulture = _previousCulture;
        }

        internal string GetString(string name, string defaultValue)
        {
            if (Data.ContainsKey(name))
                return Data[name];

            return defaultValue;
        }

        internal void SetString(string name, string value)
        {
            if (Data.ContainsKey(name))
            {
                if (Data[name] != value)
                {
                    Data[name] = value;
                    Save();
                }
            }
            else
            {
                Data[name] = value;
                Save();
            }
        }
        internal bool GetBool(string name, bool defaultValue)
        {
            if (Data.ContainsKey(name))
                return Data[name] == "1";
            else
                SetBool(name, defaultValue);

            return defaultValue;
        }
        internal void SetBool(string name, bool value)
        {
            string value_str = value ? "1" : "0";
            SetString(name, value_str);
        }
        internal int GetInt(string name, int defaultValue)
        {
            if (Data.ContainsKey(name))
            {
                if (int.TryParse(Data[name], out int value))
                {
                    return value;
                }
            }

            // invalid or no value found in Data
            SetInt(name, defaultValue);
            return defaultValue;
        }
        internal void SetInt(string name, int value)
        {
            SetString(name, value.ToString());
        }

        internal TEnum GetEnum<TEnum>(string name, TEnum defaultValue) where TEnum : struct
        {
            if (Data.ContainsKey(name))
            {
                TEnum value;
                if (Enum.TryParse<TEnum>(Data[name], out value))
                {
                    return value;
                }
            }

            return defaultValue;
        }

        internal void SetEnum<TEnum>(string name, TEnum value) where TEnum : struct
        {
            SetString(name, value.ToString());
        }

        internal float GetFloat(string name, float defaultValue)
        {
            if (Data.ContainsKey(name))
            {
                if (float.TryParse(Data[name], out float value))
                {
                    return value;
                }
            }

            // invalid or no value found in Data
            SetFloat(name, defaultValue);
            return defaultValue;
        }

        internal void SetFloat(string name, float value)
        {
            SetString(name, value.ToString());
        }
        internal double GetDouble(string name, double defaultValue)
        {
            if (Data.ContainsKey(name))
            {
                if (double.TryParse(Data[name], out double value))
                {
                    return value;
                }
            }

            // invalid or no value found in Data
            SetDouble(name, defaultValue);
            return defaultValue;
        }
        internal void SetDouble(string name, double value)
        {
            SetString(name, value.ToString());
        }

        internal Vector3 GetVector3(string name, Vector3 defaultValue)
        {
            if (!Data.ContainsKey(name))
            {
                SetParamVector3(name, defaultValue);
                return defaultValue;
            }

            string _txt = Data[name];
            string[] _ar = _txt.Split(';');

            if (_ar.Length < 3)
            {
                SetParamVector3(name, defaultValue);
                return defaultValue;
            }

            Vector3 result = Vector3.zero;
            try
            {
                result.x = float.Parse(_ar[0]);
                result.y = float.Parse(_ar[1]);
                result.z = float.Parse(_ar[2]);
            }
            catch
            {
                SetParamVector3(name, defaultValue);
                return defaultValue;
            }

            return result;
        }
        internal void SetParamVector3(string name, Vector3 value)
        {
            string _text = value.x + ";" + value.y + ";" + value.z;
            SetString(name, _text);
        }
        internal Vector3 GetVector3d(string name, Vector3d defaultValue)
        {
            if (!Data.ContainsKey(name))
            {
                SetParamVector3d(name, defaultValue);
                return defaultValue;
            }

            string _txt = Data[name];
            string[] _ar = _txt.Split(';');

            if (_ar.Length < 3)
            {
                SetParamVector3d(name, defaultValue);
                return defaultValue;
            }

            Vector3d result = Vector3d.zero;
            try
            {
                result.x = double.Parse(_ar[0]);
                result.y = double.Parse(_ar[1]);
                result.z = double.Parse(_ar[2]);
            }
            catch
            {
                SetParamVector3d(name, defaultValue);
                return defaultValue;
            }

            return result;
        }

        internal void SetParamVector3d(string name, Vector3d value)
        {
            string _text = value.x + ";" + value.y + ";" + value.z;
            SetString(name, _text);
        }
    }
}