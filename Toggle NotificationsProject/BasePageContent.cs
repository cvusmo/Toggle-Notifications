﻿using KSP.Game;
using KSP.Messages;
using System.Runtime.Remoting.Messaging;
using ToggleNotifications.TNTools.UI;
using UnityEngine;

namespace ToggleNotifications
{
    internal class BasePageContent : IPageContent
    {
        protected ToggleNotificationsPlugin mainPlugin;
        protected ToggleNotificationsUI MainUI;
        protected NotificationToggle notificationToggle;
        protected WindowTool windowTool;
        private bool uiVisible;
        private Rect windowRect = Rect.zero;
        private int windowWidth = 250;
        internal bool isGUIVisible = false;
        internal BasePageContent()
        {
            this.MainUI = ToggleNotificationsUI.Instance;
            this.mainPlugin = ToggleNotificationsPlugin.Instance;
            this.windowTool = new WindowTool();
        }
        protected MessageCenterMessage Refreshing => MainUI.Refreshing;
        protected NotificationEvents RefreshingNotification => MainUI.RefreshingNotification;

        internal virtual string Name => throw new NotImplementedException();
        internal virtual GUIContent Icon => throw new NotImplementedException();
        internal bool IsRunning => false;
        internal bool UIVisible { get => this.isGUIVisible; set => this.isGUIVisible = value; }
        internal bool IsActive => mainPlugin.interfaceEnabled;

        internal virtual void OnGUI()
        {
            Debug.Log("OnGUI BasePageContent called");
            if (!mainPlugin.isGUIVisible)
                return;

            GameState? gameState = mainPlugin.game?.GlobalGameState?.GetState();

            if (gameState == GameState.FlightView || gameState == GameState.Map3DView || gameState == GameState.Launchpad)
                {
                TNBaseStyle.Init();
                TNStyles.Init();

                Texture2D imageTexture = AssetsLoader.LoadIcon("window");
                WindowTool.CheckMainWindowPos(ref windowRect, windowWidth);
                windowRect = GUILayout.Window(
                    GUIUtility.GetControlID(FocusType.Passive),
                    windowRect,
                    mainPlugin.FillWindow,
                    "<color=#696DFF>TOGGLE NOTIFICATIONS</color>"
                );
                mainPlugin.saverectpos();
            }
        }
    }
}
