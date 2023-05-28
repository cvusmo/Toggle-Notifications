using KSP.Messages;
using KSP.Game;
using ToggleNotifications.TNTools.UI;
using UnityEngine;
using ToggleNotifications.PatchManager;

namespace ToggleNotifications.Buttons
{
    internal class ElectricityButton : MonoBehaviour
    {
        private ToggleNotificationsPlugin mainPlugin;
        private NotificationToggle notificationToggle;
        private MessageCenter messageCenter;
        private bool vesselOutOfElectricityToggle;

        public ElectricityButton(ToggleNotificationsPlugin mainPlugin, MessageCenter messageCenter, NotificationToggle notificationToggle)
        {
            this.mainPlugin = mainPlugin;
            this.messageCenter = messageCenter;
            this.notificationToggle = notificationToggle;

            vesselOutOfElectricityToggle = true;

            messageCenter.Subscribe<VesselOutOfElectricityMessage>(VesselOutOfElectricityMessageCallback);
        }

        private void VesselOutOfElectricityMessageCallback(MessageCenterMessage msg)
        {
            VesselOutOfElectricityMessage vesselOutOfElectricity = msg as VesselOutOfElectricityMessage;
            if (vesselOutOfElectricity != null)
            {
                vesselOutOfElectricityToggle = true;
            }
        }

        public void OnGUI()
        {
            int buttonWidth = Mathf.RoundToInt(mainPlugin.windowRect.width - 12);

            Rect electricityToggleRect = new Rect(3, 140, buttonWidth, 20);

            GUIStyle toggleStyle = vesselOutOfElectricityToggle ? TNBaseStyle.Toggle : TNBaseStyle.ToggleError;
            Color textColor = vesselOutOfElectricityToggle ? ColorTools.ParseColor("#C0C1E2") : Color.red;

            toggleStyle.normal.textColor = textColor;

            bool electricityToggle = GUI.Toggle(electricityToggleRect, vesselOutOfElectricityToggle, "Out of Electricity", toggleStyle);

            if (electricityToggle != vesselOutOfElectricityToggle)
            {
                if (electricityToggle)
                {
                    AssistantToTheAssistantPatchManager.isElectricalEnabled = true;
                    messageCenter.Subscribe<VesselOutOfElectricityMessage>(VesselOutOfElectricityMessageCallback);
                }
                else
                {
                    AssistantToTheAssistantPatchManager.isElectricalEnabled = false;
                    messageCenter.Unsubscribe<VesselOutOfElectricityMessage>(VesselOutOfElectricityMessageCallback);
                }

                vesselOutOfElectricityToggle = electricityToggle;
                notificationToggle.CheckCurrentState(NotificationType.VesselOutOfElectricity, vesselOutOfElectricityToggle);
            }
        }

        private void Update()
        {
            OnGUI();
        }
    }
}
