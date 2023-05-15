using BepInEx.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using UnityEngine;

namespace ToggleNotifications.TNTools
{
    public class SettingsFile
    {
        protected string FilePath = "";
        private Dictionary<string, string> Data = new Dictionary<string, string>();
        public ManualLogSource Logger = BepInEx.Logging.Logger.CreateLogSource("TN.SettingsFile");

        public SettingsFile(string file_path)
        {
            this.FilePath = file_path;
            this.Load();
        }

        protected void Load()
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            try
            {
                this.Data = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(this.FilePath));
            }
            catch (Exception ex)
            {
                this.Logger.LogWarning($"Error loading {this.FilePath}: {ex}");
            }
            Thread.CurrentThread.CurrentCulture = currentCulture;
        }
        protected void Save()
        {
            CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            try
            {
                File.WriteAllText(this.FilePath, JsonConvert.SerializeObject((object)this.Data));
            }
            catch (Exception ex)
            {
                this.Logger.LogError($"Error saving {this.FilePath}: {ex}");
            }
            Thread.CurrentThread.CurrentCulture = currentCulture;
        }
        public string GetString(string name, string defaultValue) => this.Data.ContainsKey(name) ? this.Data[name] : defaultValue;
        public void SetString(string name, string value)
        {
            if (this.Data.ContainsKey(name))
            {
                if (!(this.Data[name] != value))
                    return;
                this.Data[name] = value;
                this.Save();
            }
            else
            {
                this.Data[name] = value;
                this.Save();
            }
        }

        public bool GetBool(string name, bool defaultValue)
        {
            if (this.Data.ContainsKey(name))
                return this.Data[name] == "1";
            this.SetBool(name, defaultValue);
            return defaultValue;
        }

        public void SetBool(string name, bool value)
        {
            string str = value ? "1" : "0";
            this.SetString(name, str);
        }

        public int GetInt(string name, int defaultValue)
        {
            int result;
            if (this.Data.ContainsKey(name) && int.TryParse(this.Data[name], out result))
                return result;
            this.SetInt(name, defaultValue);
            return defaultValue;
        }

        public void SetInt(string name, int value) => this.SetString(name, value.ToString());

        public TEnum GetEnum<TEnum>(string name, TEnum defaultValue) where TEnum : struct
        {
            TEnum result;
            return this.Data.ContainsKey(name) && Enum.TryParse<TEnum>(this.Data[name], out result) ? result : defaultValue;
        }

        public void SetEnum<TEnum>(string name, TEnum value) where TEnum : struct => this.SetString(name, value.ToString());

        public float GetFloat(string name, float defaultValue)
        {
            float result;
            if (this.Data.ContainsKey(name) && float.TryParse(this.Data[name], out result))
                return result;
            this.SetFloat(name, defaultValue);
            return defaultValue;
        }

        public void SetFloat(string name, float value) => this.SetString(name, value.ToString());

        public double GetDouble(string name, double defaultValue)
        {
            double result;
            if (this.Data.ContainsKey(name) && double.TryParse(this.Data[name], out result))
                return result;
            this.SetDouble(name, defaultValue);
            return defaultValue;
        }

        public void SetDouble(string name, double value) => this.SetString(name, value.ToString());

        public Vector3 GetVector3(string name, Vector3 defaultValue)
        {
            if (!this.Data.ContainsKey(name))
            {
                this.SetParamVector3(name, defaultValue);
                return defaultValue;
            }
            string[] strArray = this.Data[name].Split(';');
            if (strArray.Length < 3)
            {
                this.SetParamVector3(name, defaultValue);
                return defaultValue;
            }
            Vector3 zero = Vector3.zero;
            try
            {
                zero.x = float.Parse(strArray[0]);
                zero.y = float.Parse(strArray[1]);
                zero.z = float.Parse(strArray[2]);
            }
            catch
            {
                this.SetParamVector3(name, defaultValue);
                return defaultValue;
            }
            return zero;
        }

        public void SetParamVector3(string name, Vector3 value)
        {
            string str = value.x.ToString() + ";" + value.y.ToString() + ";" + value.z.ToString();
            this.SetString(name, str);
        }

        public Vector3 GetVector3d(string name, Vector3d defaultValue)
        {
            if (!this.Data.ContainsKey(name))
            {
                this.SetParamVector3d(name, defaultValue);
                return (Vector3)defaultValue;
            }
            string[] strArray = this.Data[name].Split(';');
            if (strArray.Length < 3)
            {
                this.SetParamVector3d(name, defaultValue);
                return (Vector3)defaultValue;
            }
            Vector3d zero = Vector3d.zero;
            try
            {
                zero.x = double.Parse(strArray[0]);
                zero.y = double.Parse(strArray[1]);
                zero.z = double.Parse(strArray[2]);
            }
            catch
            {
                this.SetParamVector3d(name, defaultValue);
                return (Vector3)defaultValue;
            }
            return (Vector3)zero;
        }

        public void SetParamVector3d(string name, Vector3d value)
        {
            string str = value.x.ToString() + ";" + value.y.ToString() + ";" + value.z.ToString();
            this.SetString(name, str);
        }
    }
}
