using SpaceWarp.API.Assets;
using ToggleNotifications.Controller;
using UnityEngine;

namespace ToggleNotifications.Pages
{
    internal class MainPage : BaseController
    {
        public string Name => "Toggle Notifications";
        readonly Texture2D tabIcon = AssetManager.GetAsset<Texture2D>($"{ToggleNotificationsPlugin.Instance.SpaceWarpMetadata.ModID}/images/tn_icon_50.png");
        public override GUIContent Icon => new(tabIcon, "Toggle Notifications");

        public override void OnGUI()
        {
            TNStyles.DrawSectionHeader("Toggle Notifications");
        }
    }
}
