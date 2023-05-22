﻿using BepInEx.Configuration;

namespace ToggleNotifications
{
    public class TNSettings
    {
        private static ConfigFile s_config_file;
        public static void Init(string configurationpath)
        {
            s_config_file = new ConfigFile(configurationpath, true);
        }

        public static T GetValue<T>(string key, T defaultValue)
        {
            return s_config_file.Bind("", key, defaultValue).Value;
        }

        public static void SetValue<T>(string key, T value)
        {
            s_config_file.Bind("", key, value).Value = value;
            s_config_file.Save();
        }
        public static bool SolarPanelsIneffectiveMessage
        {
            get => GetValue(nameof(SolarPanelsIneffectiveMessage), true);
            set { SetValue(nameof(SolarPanelsIneffectiveMessage), value); }
        }

        public static bool SolarPanelsIneffectiveTimeToWaitTill
        {
            get => GetValue(nameof(SolarPanelsIneffectiveTimeToWaitTill), true);
            set { SetValue(nameof(SolarPanelsIneffectiveTimeToWaitTill), value); }
        }

        public static bool VesselLeftCommunicationRangeMessage
        {
            get => GetValue(nameof(VesselLeftCommunicationRangeMessage), true);
            set { SetValue(nameof(VesselLeftCommunicationRangeMessage), value); }
        }

        public static bool VesselThrottleLockedDueToTimewarpingMessage
        {
            get => GetValue(nameof(VesselThrottleLockedDueToTimewarpingMessage), true);
            set { SetValue(nameof(VesselThrottleLockedDueToTimewarpingMessage), value); }
        }

        public static bool CannotPlaceManeuverNodeWhileOutOfFuelMessage
        {
            get => GetValue(nameof(CannotPlaceManeuverNodeWhileOutOfFuelMessage), true);
            set { SetValue(nameof(CannotPlaceManeuverNodeWhileOutOfFuelMessage), value); }
        }

        public static bool GamePauseToggledMessage
        {
            get => GetValue(nameof(GamePauseToggledMessage), true);
            set { SetValue(nameof(GamePauseToggledMessage), value); }
        }
    }
}
