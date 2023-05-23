using KSP.Messages;
using ToggleNotifications.TNTools.UI;
using UnityEngine;

namespace ToggleNotifications
{
    internal class SolarPanelButton
    {
        private ToggleNotificationsPlugin mainPlugin;
        private MessageCenter messageCenter;
        private SubscriptionHandle onActionActivateMessageHandle;
        private bool isSolarPanelEnabled;

        public SolarPanelButton(ToggleNotificationsPlugin mainPlugin, MessageCenter messageCenter)
        {
            this.mainPlugin = mainPlugin;
            this.messageCenter = messageCenter;
            isSolarPanelEnabled = false;

            // Subscribe to the SolarPanelsIneffectiveMessage and define the callback logic
            onActionActivateMessageHandle = messageCenter.Subscribe<SolarPanelsIneffectiveMessage>(SolarPanelsIneffectiveMessageCallback);
        }

        private void SolarPanelsIneffectiveMessageCallback(MessageCenterMessage msg)
        {
           //tbd
        }

        public void RenderToggleSolar()
        {
            int buttonWidth = Mathf.RoundToInt(mainPlugin.windowRect.width - 12); // Subtract 3 on each side for padding
            Rect solarPanelToggleRect = new Rect(3, 96, buttonWidth, 20);

            GUIStyle toggleStyle = isSolarPanelEnabled ? TNBaseStyle.Toggle : TNBaseStyle.ToggleError;
            Color textColor = isSolarPanelEnabled ? ColorTools.ParseColor("#C0C1E2") : ColorTools.ParseColor("#C0E2DC");

            isSolarPanelEnabled = GUI.Toggle(solarPanelToggleRect, isSolarPanelEnabled, "Solar Panel Ineffective", toggleStyle);
            toggleStyle.normal.textColor = textColor;

            if (isSolarPanelEnabled)
            {
                // Solar panels are ineffective
                mainPlugin.notificationToggle.CheckCurrentState(NotificationType.SolarPanelsIneffectiveMessage, false);
                AssistantToTheAssistantPatchManager.isSolarPanelsEnabled = false;
                messageCenter.Unsubscribe<SolarPanelsIneffectiveMessage>(SolarPanelsIneffectiveMessageCallback);
            }
            else
            {
                // Solar panels are effective
                AssistantToTheAssistantPatchManager.isSolarPanelsEnabled = true;

                // Unsubscribe from the SolarPanelsIneffectiveMessage
                messageCenter.Unsubscribe<SolarPanelsIneffectiveMessage>(SolarPanelsIneffectiveMessageCallback);

                // Create and publish the SolarPanelsIneffectiveMessage
                SolarPanelsIneffectiveMessage message = new SolarPanelsIneffectiveMessage();
                messageCenter.Publish<SolarPanelsIneffectiveMessage>(message);

                // Subscribe to the SolarPanelsIneffectiveMessage
                onActionActivateMessageHandle = messageCenter.Subscribe<SolarPanelsIneffectiveMessage>(SolarPanelsIneffectiveMessageCallback);

                // Set the GUI style for TNBaseStyle.Toggle
                TNBaseStyle.Toggle.normal.textColor = ColorTools.ParseColor("#C0C1E2");
                TNBaseStyle.ToggleError.normal.textColor = ColorTools.ParseColor("#C0E2DC");
            }
        }
    }
}
