using KSP.Messages;
using KSP.Game;
using ToggleNotifications.TNTools.UI;
using UnityEngine;
using ToggleNotifications.PatchManager;

namespace ToggleNotifications.Buttons
{
    internal class VesselLostControlButton : MonoBehaviour
    {
        private ToggleNotificationsPlugin mainPlugin;
        private NotificationToggle notificationToggle;
        private MessageCenter messageCenter;
        private bool vesselLostControlToggle;

        public VesselLostControlButton(ToggleNotificationsPlugin mainPlugin, MessageCenter messageCenter, NotificationToggle notificationToggle)
        {
            this.mainPlugin = mainPlugin;
            this.messageCenter = messageCenter;
            this.notificationToggle = notificationToggle;

            vesselLostControlToggle = true;

            messageCenter.Subscribe<VesselLostControlMessage>(VesselLostControlMessageCallback);
        }

        private void VesselLostControlMessageCallback(MessageCenterMessage msg)
        {
            VesselLostControlMessage lostControlToggle = msg as VesselLostControlMessage;
            if (lostControlToggle != null)
            {
                vesselLostControlToggle = true;
            }
        }

        public void OnGUI()
        {
            int buttonWidth = Mathf.RoundToInt(mainPlugin.windowRect.width - 12);

            Rect lostControlToggleRect = new Rect(3, 180, buttonWidth, 20);

            GUIStyle toggleStyle = vesselLostControlToggle ? TNBaseStyle.Toggle : TNBaseStyle.ToggleError;
            Color textColor = vesselLostControlToggle ? ColorTools.ParseColor("#C0C1E2") : Color.red;

            toggleStyle.normal.textColor = textColor;

            bool lostControlToggle = GUI.Toggle(lostControlToggleRect, vesselLostControlToggle, "Vessel Lost Control", toggleStyle);

            if (lostControlToggle != vesselLostControlToggle)
            {
                if (lostControlToggle)
                {
                    AssistantToTheAssistantPatchManager.isLostControlEnabled = true;
                    messageCenter.Subscribe<VesselLostControlMessage>(VesselLostControlMessageCallback);
                }
                else
                {
                    AssistantToTheAssistantPatchManager.isLostControlEnabled = false;
                    messageCenter.Unsubscribe<VesselLostControlMessage>(VesselLostControlMessageCallback);
                }

                vesselLostControlToggle = lostControlToggle;
                notificationToggle.CheckCurrentState(NotificationType.VesselLostControlMessage, vesselLostControlToggle);
            }
        }

        private void Update()
        {
            OnGUI();
        }
    }
}
