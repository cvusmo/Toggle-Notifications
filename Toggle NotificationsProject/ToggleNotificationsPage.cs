using UnityEngine;

namespace ToggleNotifications
{
    public class ToggleNotificationsPage : BasePageContent
    {
        public int PageIndex { get; set; }
        private string _targetAltitude = "600";
        private readonly Texture2D tabIcon = null; // Replace with your icon texture

        public override string Name => "Toggle Notifications";

        public override GUIContent Icon => new GUIContent(tabIcon, "Toggle Notifications");

        public override bool IsActive => true;

        public override void OnGUI()
        {
            GUILayout.Label($"Page {PageIndex + 1}: Toggle Notifications");
            // Add your UI elements here
            GUILayout.Label("Target Altitude: " + _targetAltitude);

            // Add more UI elements as needed

            HandleButtons();
        }

        private void HandleButtons()
        {
            // Add logic to handle button clicks and update variables
            // For example:
            if (GUILayout.Button("Increase Target Altitude"))
            {
                int altitude = int.Parse(_targetAltitude);
                altitude += 100;
                _targetAltitude = altitude.ToString();
            }
        }
    }
}
