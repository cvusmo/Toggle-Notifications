using BepInEx;
using BepInEx.Logging;
using Newtonsoft.Json;
using UnityEngine;

namespace ToggleNotifications.Tools;

public class SettingsFile
{
    protected string file_path = "";
    Dictionary<string, string> data = new Dictionary<string, string>();

    public SettingsFile()
    {
        file_path = Path.Combine(Paths.ConfigPath, "ToggleNotifications.cfg");
        Load();
    }

    public ManualLogSource logger = BepInEx.Logging.Logger.CreateLogSource("TNStyles.SettingsFile");

    protected void Load()
    {
        var previous_culture = Thread.CurrentThread.CurrentCulture;
        Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
        try
        {
            if (File.Exists(file_path))
            {
                data = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(file_path));
            }
            else
            {
                logger.LogWarning($"Settings file {file_path} does not exist.");
            }
        }
        catch (Exception)
        {
            logger.LogWarning($"Error loading settings from {file_path}");
        }

        Thread.CurrentThread.CurrentCulture = previous_culture;
    }

    protected void Save()
    {
        var previous_culture = Thread.CurrentThread.CurrentCulture;
        Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;
        try
        {
            File.WriteAllText(file_path, JsonConvert.SerializeObject(data));
        }
        catch (Exception)
        {
            logger.LogError($"error saving {file_path}");
        }

        Thread.CurrentThread.CurrentCulture = previous_culture;
    }

    public string GetString(string name, string defaultValue)
    {
        if (data.ContainsKey(name))
            return data[name];

        return defaultValue;
    }

    public void SetString(string name, string value)
    {
        if (data.ContainsKey(name))
        {
            if (data[name] != value)
            {
                data[name] = value;
                Save();
            }
        }
        else
        {
            data[name] = value;
            Save();
        }
    }

    public T Get<T>(string name, T defaultValue, string section = "Settings")
    {
        if (data.ContainsKey(name))
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(data[name]);
            }
            catch (Exception)
            {
                logger.LogWarning($"error deserializing {name}");
            }
        }
        Set(name, defaultValue);
        return defaultValue;
    }

    public void Set<T>(string name, T value, string section = "Settings")
    {
        string serializedValue = JsonConvert.SerializeObject(value);
        if (data.ContainsKey(name))
        {
            if (data[name] != serializedValue)
            {
                data[name] = serializedValue;
                Save();
            }
        }
        else
        {
            data[name] = serializedValue;
            Save();
        }
    }


    /// <summary>
    /// Get the parameter using bool value
    /// if not found it is added and saved at once
    /// </summary>
    public bool GetBool(string name, bool defaultValue)
    {
        if (data.ContainsKey(name))
            return data[name] == "1";
        else
            SetBool(name, defaultValue);

        return defaultValue;
    }

    /// <summary>
    /// Set the parameter using bool value
    /// the value is saved at once
    /// </summary>
    public void SetBool(string name, bool value)
    {
        string value_str = value ? "1" : "0";
        SetString(name, value_str);
    }


    /// <summary>
    /// Get the parameter using integer value
    /// if not found or on parsing error, it is replaced and saved at once
    /// </summary>
    public int GetInt(string name, int defaultValue)
    {
        if (data.ContainsKey(name))
        {
            int value = 0;
            if (int.TryParse(data[name], out value))
            {
                return value;
            }
        }

        // invalid or no value found in data
        SetInt(name, defaultValue);
        return defaultValue;
    }

    /// <summary>
    /// Set the parameter using integer value
    /// the value is saved at once
    /// </summary>
    public void SetInt(string name, int value)
    {
        SetString(name, value.ToString());
    }

    public TEnum GetEnum<TEnum>(string name, TEnum defaultValue) where TEnum : struct
    {
        if (data.ContainsKey(name))
        {
            TEnum value = defaultValue;
            if (Enum.TryParse(data[name], out value))
            {
                return value;
            }
        }

        return defaultValue;
    }

    public void SetEnum<TEnum>(string name, TEnum value) where TEnum : struct
    {
        SetString(name, value.ToString());
    }


    /// <summary>
    /// Get the parameter using float value
    /// if not found or on parsing error, it is replaced and saved at once
    /// </summary>
    public float GetFloat(string name, float defaultValue)
    {
        if (data.ContainsKey(name))
        {
            float value = 0;
            if (float.TryParse(data[name], out value))
            {
                return value;
            }
        }

        // invalid or no value found in data
        SetFloat(name, defaultValue);
        return defaultValue;
    }

    /// <summary>
    /// Set the parameter using float value
    /// the value is saved at once
    /// </summary>
    public void SetFloat(string name, float value)
    {
        SetString(name, value.ToString());
    }

    /// <summary>
    /// Get the parameter using double value
    /// if not found or on parsing error, it is replaced and saved at once
    /// </summary>
    public double GetDouble(string name, double defaultValue)
    {
        if (data.ContainsKey(name))
        {
            double value = 0;
            if (double.TryParse(data[name], out value))
            {
                SetDouble(name, value);
                return value;
            }
        }

        // invalid or no value found in data
        SetDouble(name, defaultValue);
        return defaultValue;
    }

    /// <summary>
    /// Set the parameter using double value
    /// the value is saved at once
    /// </summary>
    public void SetDouble(string name, double value)
    {
        SetString(name, value.ToString());
    }

    /// <summary>
    /// Get the parameter using Vector3 value
    ///  if not found or on parsing error, it is replaced and saved at once
    /// </summary>
    public Vector3 GetVector3(string name, Vector3 defaultValue)
    {
        if (!data.ContainsKey(name))
        {
            SetParamVector3(name, defaultValue);
            return defaultValue;
        }

        string txt = data[name];
        string[] ar = txt.Split(';');

        if (ar.Length < 3)
        {
            SetParamVector3(name, defaultValue);
            return defaultValue;
        }

        Vector3 result = Vector3.zero;
        try
        {
            result.x = float.Parse(ar[0]);
            result.y = float.Parse(ar[1]);
            result.z = float.Parse(ar[2]);
        }
        catch
        {
            SetParamVector3(name, defaultValue);
            return defaultValue;
        }

        return result;
    }

    /// <summary>
    /// Set the parameter using Vector3 value
    /// the value is saved at once
    /// </summary>
    public void SetParamVector3(string name, Vector3 value)
    {
        string text = value.x + ";" + value.y + ";" + value.z;
        SetString(name, text);
    }

    /// <summary>
    /// Get the parameter using Vector3d value
    ///  if not found or on parsing error, it is replaced and saved at once
    /// </summary>
    public Vector3 GetVector3d(string name, Vector3d defaultValue)
    {
        if (!data.ContainsKey(name))
        {
            SetParamVector3d(name, defaultValue);
            return defaultValue;
        }

        string txt = data[name];
        string[] ar = txt.Split(';');

        if (ar.Length < 3)
        {
            SetParamVector3d(name, defaultValue);
            return defaultValue;
        }

        Vector3d result = Vector3d.zero;
        try
        {
            result.x = double.Parse(ar[0]);
            result.y = double.Parse(ar[1]);
            result.z = double.Parse(ar[2]);
        }
        catch
        {
            SetParamVector3d(name, defaultValue);
            return defaultValue;
        }

        return result;
    }

    public void SetParamVector3d(string name, Vector3d value)
    {
        string text = value.x + ";" + value.y + ";" + value.z;
        SetString(name, text);
    }
}