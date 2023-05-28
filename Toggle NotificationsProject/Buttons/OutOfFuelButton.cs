using KSP.Messages;
using KSP.Game;
using ToggleNotifications.TNTools.UI;
using UnityEngine;
using ToggleNotifications.PatchManager;

namespace ToggleNotifications.Buttons
{
    internal class OutOfFuelButton : MonoBehaviour
    {
        private ToggleNotificationsPlugin mainPlugin;
        private NotificationToggle notificationToggle;
        private MessageCenter messageCenter;
        private bool outOfFuelToggle;

        public OutOfFuelButton(ToggleNotificationsPlugin mainPlugin, MessageCenter messageCenter, NotificationToggle notificationToggle)
        {
            this.mainPlugin = mainPlugin;
            this.messageCenter = messageCenter;
            this.notificationToggle = notificationToggle;

            outOfFuelToggle = AssistantToTheAssistantPatchManager.isOutOfFuelEnabled;

            messageCenter.Subscribe<CannotPlaceManeuverNodeWhileOutOfFuelMessage>(CannotPlaceManeuverNodeWhileOutOfFuelMessageCallback);
            messageCenter.Subscribe<CannotChangeNodeWhileOutOfFuelMessage>(CannotChangeNodeWhileOutOfFuelMessageCallback);
        }

        private void CannotPlaceManeuverNodeWhileOutOfFuelMessageCallback(MessageCenterMessage msg)
        {
            CannotPlaceManeuverNodeWhileOutOfFuelMessage cannotPlaceManeuverNode = msg as CannotPlaceManeuverNodeWhileOutOfFuelMessage;
            if (cannotPlaceManeuverNode != null)
            {
                outOfFuelToggle = AssistantToTheAssistantPatchManager.isOutOfFuelEnabled;
            }
        }

        private void CannotChangeNodeWhileOutOfFuelMessageCallback(MessageCenterMessage msg)
        {
            CannotChangeNodeWhileOutOfFuelMessage cannotChangeNode = msg as CannotChangeNodeWhileOutOfFuelMessage;
            if (cannotChangeNode != null)
            {
                outOfFuelToggle = AssistantToTheAssistantPatchManager.isOutOfFuelEnabled;
            }
        }

        public void OnGUI()
        {
            int buttonWidth = Mathf.RoundToInt(mainPlugin.windowRect.width - 12);

            Rect outOfFuelToggleRect = new Rect(3, 260, buttonWidth, 20);

            GUIStyle toggleStyle = outOfFuelToggle ? TNBaseStyle.Toggle : TNBaseStyle.ToggleError;
            Color textColor = outOfFuelToggle ? ColorTools.ParseColor("#C0C1E2") : Color.red;

            toggleStyle.normal.textColor = textColor;

            bool cannotChangeOutOfFuelToggle = GUI.Toggle(outOfFuelToggleRect, outOfFuelToggle, "Cannot Change MN Out of Fuel", toggleStyle);

            if (cannotChangeOutOfFuelToggle != outOfFuelToggle)
            {
                outOfFuelToggle = cannotChangeOutOfFuelToggle;
                AssistantToTheAssistantPatchManager.isOutOfFuelEnabled = outOfFuelToggle;

                if (outOfFuelToggle)
                {
                    messageCenter.Subscribe<CannotPlaceManeuverNodeWhileOutOfFuelMessage>(CannotPlaceManeuverNodeWhileOutOfFuelMessageCallback);
                    messageCenter.Subscribe<CannotChangeNodeWhileOutOfFuelMessage>(CannotChangeNodeWhileOutOfFuelMessageCallback);
                }
                else
                {
                    messageCenter.Unsubscribe<CannotPlaceManeuverNodeWhileOutOfFuelMessage>(CannotPlaceManeuverNodeWhileOutOfFuelMessageCallback);
                    messageCenter.Unsubscribe<CannotChangeNodeWhileOutOfFuelMessage>(CannotChangeNodeWhileOutOfFuelMessageCallback);
                }

                notificationToggle.CheckCurrentState(NotificationType.CannotPlaceManeuverNodeWhileOutOfFuelMessage, outOfFuelToggle);
                notificationToggle.CheckCurrentState(NotificationType.CannotChangeNodeWhileOutOfFuelMessage, outOfFuelToggle);
            }
        }

        private void Update()
        {
            OnGUI();
        }
    }
}
