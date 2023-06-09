﻿using KSP.Messages;
using ToggleNotifications.PatchManager;
using ToggleNotifications.TNTools.UI;
using UnityEngine;

namespace ToggleNotifications.Buttons
{
    internal class SolarPanelButton : MonoBehaviour
    {
        private ToggleNotificationsPlugin mainPlugin;
        private NotificationToggle notificationToggle;
        private MessageCenter messageCenter;
        private bool solarPanelNotificationEnabled;

        public SolarPanelButton(ToggleNotificationsPlugin mainPlugin, MessageCenter messageCenter, NotificationToggle notificationToggle)
        {
            this.mainPlugin = mainPlugin;
            this.messageCenter = messageCenter;
            this.notificationToggle = notificationToggle;

            solarPanelNotificationEnabled = true;

            messageCenter.Subscribe<SolarPanelsIneffectiveMessage>(SolarPanelsIneffectiveMessageCallback);
        }

        private void SolarPanelsIneffectiveMessageCallback(MessageCenterMessage msg)
        {
            SolarPanelsIneffectiveMessage solarPanelsIneffective = msg as SolarPanelsIneffectiveMessage;
            if (solarPanelsIneffective != null)
            {
                solarPanelNotificationEnabled = true;
            }
        }

        public void OnGUI()
        {
            int buttonWidth = Mathf.RoundToInt(mainPlugin.windowRect.width - 12);
            Rect solarPanelToggleRect = new Rect(3, 100, buttonWidth, 20);

            GUIStyle toggleStyle = solarPanelNotificationEnabled ? TNBaseStyle.Toggle : TNBaseStyle.ToggleError;
            Color textColor = solarPanelNotificationEnabled ? ColorTools.ParseColor("#C0C1E2") : Color.red;

            toggleStyle.normal.textColor = textColor;

            bool solarPanelToggle = GUI.Toggle(solarPanelToggleRect, solarPanelNotificationEnabled, "Solar Panel Ineffective", toggleStyle);

            if (solarPanelToggle != solarPanelNotificationEnabled)
            {
                if (solarPanelToggle)
                {
                    AssistantToTheAssistantPatchManager.isSolarPanelsEnabled = true;
                    messageCenter.Subscribe<SolarPanelsIneffectiveMessage>(SolarPanelsIneffectiveMessageCallback);
                }
                else
                {
                    AssistantToTheAssistantPatchManager.isSolarPanelsEnabled = false;
                    messageCenter.Unsubscribe<SolarPanelsIneffectiveMessage>(SolarPanelsIneffectiveMessageCallback);
                }

                solarPanelNotificationEnabled = solarPanelToggle;
                notificationToggle.CheckCurrentState(NotificationType.SolarPanelsIneffectiveMessage, solarPanelNotificationEnabled);
            }
        }

        private void Update()
        {
            OnGUI();
        }
    }
}
