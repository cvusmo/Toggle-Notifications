using KSP.Messages;
using ToggleNotifications.PatchManager;
using ToggleNotifications.TNTools.UI;
using UnityEngine;

namespace ToggleNotifications.Buttons
{
    internal class CommunicationRangeButton : MonoBehaviour
    {
        private ToggleNotificationsPlugin mainPlugin;
        private NotificationToggle notificationToggle;
        private MessageCenter messageCenter;
        private bool vesselOutOfCommRangeToggle;

        public CommunicationRangeButton(ToggleNotificationsPlugin mainPlugin, MessageCenter messageCenter, NotificationToggle notificationToggle)
        {
            this.mainPlugin = mainPlugin;
            this.messageCenter = messageCenter;
            this.notificationToggle = notificationToggle;

            vesselOutOfCommRangeToggle = true;

            messageCenter.Subscribe<VesselLeftCommunicationRangeMessage>(VesselLeftCommunicationRangeMessageCallback);
        }

        private void VesselLeftCommunicationRangeMessageCallback(MessageCenterMessage msg)
        {
            VesselLeftCommunicationRangeMessage vesselOutOfCommRange = msg as VesselLeftCommunicationRangeMessage;
            if (vesselOutOfCommRange != null)
            {
                vesselOutOfCommRangeToggle = true;
            }
        }

        public void OnGUI()
        {
            int buttonWidth = Mathf.RoundToInt(mainPlugin.windowRect.width - 12);

            Rect commRangeToggleRect = new Rect(3, 220, buttonWidth, 20);

            GUIStyle toggleStyle = vesselOutOfCommRangeToggle ? TNBaseStyle.Toggle : TNBaseStyle.ToggleError;
            Color textColor = vesselOutOfCommRangeToggle ? ColorTools.ParseColor("#C0C1E2") : Color.red;

            toggleStyle.normal.textColor = textColor;

            bool commRangeToggle = GUI.Toggle(commRangeToggleRect, vesselOutOfCommRangeToggle, "Out of Communication Range", toggleStyle);

            if (commRangeToggle != vesselOutOfCommRangeToggle)
            {
                if (commRangeToggle)
                {
                    AssistantToTheAssistantPatchManager.isCommRangeEnabled = true;
                    messageCenter.Subscribe<VesselLeftCommunicationRangeMessage>(VesselLeftCommunicationRangeMessageCallback);
                }
                else
                {
                    AssistantToTheAssistantPatchManager.isCommRangeEnabled = false;
                    messageCenter.Unsubscribe<VesselLeftCommunicationRangeMessage>(VesselLeftCommunicationRangeMessageCallback);
                }

                vesselOutOfCommRangeToggle = commRangeToggle;
                notificationToggle.CheckCurrentState(NotificationType.VesselLeftCommunicationRangeMessage, vesselOutOfCommRangeToggle);
            }
        }

        private void Update()
        {
            OnGUI();
        }
    }
}
