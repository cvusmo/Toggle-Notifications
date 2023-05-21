using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using KSP.Game;
using KSP.UI.Binding;
using SpaceWarp;
using SpaceWarp.API.Assets;
using SpaceWarp.API.Mods;
using SpaceWarp.API.UI.Appbar;
using System.Reflection;
using ToggleNotifications.TNTools;
using ToggleNotifications.TNTools.UI;
using UnityEngine;
using Color = UnityEngine.Color;

namespace ToggleNotifications
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    [BepInDependency(SpaceWarpPlugin.ModGuid, SpaceWarpPlugin.ModVer)]
    public class ToggleNotificationsPlugin : BaseSpaceWarpPlugin
    {
        //modinfo
        public const string ModGuid = MyPluginInfo.PLUGIN_GUID;
        public const string ModName = MyPluginInfo.PLUGIN_NAME;
        public const string ModVer = MyPluginInfo.PLUGIN_VERSION;

        //core
        internal ToggleNotificationsUI MainUI;
        internal NotificationToggle notificationToggle;
        internal bool interfaceEnabled;
        internal bool isGUIVisible = false;
        internal Rect windowRect = Rect.zero;
        internal int windowWidth = 250;
        internal GameInstance game;
        
        //config
        public ConfigEntry<bool> tnConfig;
        protected bool defaultValue;

        //appbar
        private const string ToolbarFlightButtonID = "BTN-ToggleNotificationsFlight";
        private static string assemblyFolder;
        private static string settingsPath;
        private static string AssemblyFolder => assemblyFolder ?? (assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
        private static string SettingsPath => settingsPath ?? (settingsPath = Path.Combine(AssemblyFolder, "settings.json"));
        
        internal static ToggleNotificationsPlugin Instance { get; private set; }
        internal new static ManualLogSource Logger { get; set; }
        public override void OnInitialized()
        {
            TNBaseSettings.Init(SettingsPath);

            base.OnInitialized();
            
            Instance = this;
            Logger = base.Logger;
            Logger.LogInfo("Loaded");
            MainUI = new ToggleNotificationsUI(this, isGUIVisible);

            game = GameManager.Instance.Game;

            // Register Flight AppBar button
            Appbar.RegisterAppButton(
                "Toggle Notifications",
                ToolbarFlightButtonID,
                AssetManager.GetAsset<Texture2D>($"{this.SpaceWarpMetadata.ModID}/images/icon.png"),
                isOpen =>
                {
                    ToggleButton(isOpen, isOpen);
                    Debug.Log($"Initial isWindowOpen value: {interfaceEnabled}");
                }
            );

            // configuration
           tnConfig = Config.Bind("Notification Settings", "Toggle Notifications", defaultValue, "Toggle Notifications is a mod that allows you to enable or disable notifications");
          defaultValue = tnConfig.Value;
           tnConfig.Value = true;

            AssistantToTheAssistantPatchManager.ApplyPatches(notificationToggle);
        }


        internal void ToggleButton(bool toggle, bool isOpen)
        {
            interfaceEnabled = isOpen;
            isGUIVisible = toggle;
            GameObject.Find("BTN-ToggleNotificationsFlight")?.GetComponent<UIValue_WriteBool_Toggle>()?.SetValue(isGUIVisible);
        }
        internal void Update()
        {
            if (Input.GetKey(KeyCode.RightAlt) && Input.GetKeyDown(KeyCode.P))
            {
                this.ToggleButton(!this.interfaceEnabled, !this.isGUIVisible);
                Logger.LogInfo("Update: Toggle Notifications UI toggled with hotkey");
            }
            if (isGUIVisible)
            {
                if (MainUI == null)
                    return;
                MainUI.Update();
            }
        }
        internal void saverectpos()
        {
            TNBaseSettings.WindowXPos = (int)windowRect.xMin;
            TNBaseSettings.WindowYPos = (int)windowRect.yMin;
        }
        private void OnGUI()
        {
            if (!isGUIVisible)
                return;

            TNStyles.Init();
            WindowTool.CheckMainWindowPos(ref windowRect, windowWidth);
            GUI.skin = TNBaseStyle.Skin;

            this.windowRect = GUILayout.Window(
                GUIUtility.GetControlID(FocusType.Passive),
                this.windowRect,
                MainUI.FillWindow, // Call the FillWindow method of the MainUI instance
                "<color=#696DFF>TOGGLE NOTIFICATIONS</color>",
                GUILayout.Height(0.0f),
                GUILayout.Width((float)this.windowWidth),
                GUILayout.MinHeight(400) // Adjust the value to your desired height
            );

            saverectpos();
        }    
        internal void CloseWindow()
        {
            GameObject.Find("BTN-ToggleNotificationsFlight")?.GetComponent<UIValue_WriteBool_Toggle>()?.SetValue(false);
            interfaceEnabled = false;
            ToggleButton(false, false);
            Rect closeButtonPosition = new Rect(windowRect.width - 30, 4f, 23f, 23f);
            TopButtons.SetPosition(closeButtonPosition);
        }
    }
}