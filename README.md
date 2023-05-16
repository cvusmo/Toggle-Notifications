Toggle Notifications
This project is to Toggle Notificaitons on or off.

Thank you to LuxStice, ShadowDev, Munix, The Yeetster, and schlosrat for all the help getting this working correctly!

Requirements
SpaceWarp + BepInEx + KSP 2


INSTALLATION:
Three options:

CKAN
Spacedock.info
Manual
1. CKAN
Make sure you've updated CKAN to the current release. If you don't have CKAN installed, go here: https://github.com/KSP-CKAN/CKAN/releases Install CKAN and make sure you have KSP2 as the game instance. You'll see the option to click on Toggle Notifications. Apply changes.

2. Spacedock.info
https://spacedock.info/kerbal-space-program-2 Download zip Drag contents of zip to BepInEx/plugins/ Place ToggleNotifications inside plugins

3. Manual
Download the current release from github

Versioning
The template versions follow this convention: x.y.z.version, where x.y.z is the corresponding ToggleNotifications version and version is the template version for that specific ToggleNotifications release. For example, 0.1.0 is the first version.

Functionality:
ToggleNotifications Plugin 


The plugin is called "ToggleNotifications" and allows the player to toggle various in-game notifications on or off.

The code includes a variety of functions and variables related to the plugin's functionality. These include the ability to enable or disable notifications, track the current state of each notification, and update the GUI to reflect changes in the player's settings. The plugin also includes code to add a button to the in-game toolbar, allowing the player to toggle notifications on or off from within the game.

Overall, the plugin provides a convenient way for players to customize their in-game experience and control the flow of information they receive during gameplay.

The code contains several classes and interfaces that are related to the selection of different types of notifications.

The SolarPanelSelection class implements the SetSelection interface and contains the logic for selecting whether or not to receive notifications about solar panels.
It has a reference to the ToggleNotificationsUI object, which is the main UI object for the plugin, and a solarOption object that represents the current selection for solar panel notifications.
The GetOptionType() method is used to get the type of notification selected (in this case, the solar panel notification), and the SetOption() method is used to set the state of the notification toggle.
The GetContent() method returns the content for the solar panel selection, and SolarTypeSelectionGUI() is the GUI method for selecting whether or not to receive solar panel notifications.
The DrawContents() method is used to draw the contents of the solar panel selection.
There are other classes and interfaces in the code as well, such as CommRangeSelection, ThrottleLockedSelection, ManeuverNodeOutOfFuelSelection, GamePauseToggledSelection, and AllNotificationsSelection. These classes contain similar logic for selecting different types of notifications.

The Selection class is the main class that holds the dictionary of SetSelection objects for all the different notification types. It contains the logic for creating the SetSelection objects and adding them to the dictionary.

Overall, the code is designed to allow the user to select which types of notifications they want to receive and to control the state of the corresponding notification toggles.

ToggleNotificationsUI: This is the main class of the plugin that handles the UI and state for toggling notifications.

SetSelection: This class appears to be a container for a group of selection objects. Each selection object seems to represent a specific type of notification that can be toggled on and off.

ToggleNotificationsPlugin: This is likely the main plugin object that interacts with the game. It probably handles the actual function of enabling and disabling the notifications.

Update(): This method is likely called each frame by Unity to update the state of the plugin. It probably checks the current state of each notification and updates the UI accordingly.

DrawEntryButton, DrawEntry2Button, DrawLabelWithTextField, etc.: These methods are likely used to draw the UI components that allow the user to toggle the different notification settings.

CommRangeTypeSelectionGUI, ThrottleLockedWarpTypeSelectionGUI, ManeuverNodeOutOfFuelSelectionGUI, etc.: These methods are probably used to draw the selection GUIs for each of the different notification types.

OnGUI(): This is a Unity-specific method that is called to draw the plugin's UI. It looks like it's creating and updating the tabs for the various notification settings and running the refresh state method in a separate thread to avoid blocking the UI.

UpdateCurrentStates sets the state of each notification type based on the corresponding UI element state.

SetCurrentState sets the state of all notifications to a single value based on the allNotificationsState field.

SetAllNotificationsState is a public method that sets the state of all notifications to a single value based on the argument passed in.

SetSolarPanelState, SetCommunicationRangeState, SetThrottleLockedWarpState, SetManeuverNodeOutOfFuelState, and SetGamePauseToggledState are public methods that set the state of individual notifications based on the argument passed in.


SetSolarPanelState(bool state): This method is used to enable or disable the solar panel ineffective message notification. It sets the notification state for the "SolarPanelsIneffectiveMessage" notification.

SetCommunicationRangeState(bool state): This method is used to enable or disable the vessel left communication range message notification. It sets the notification state for the "VesselLeftCommunicationRangeMessage" notification.

SetThrottleLockedWarpState(bool state): This method is used to enable or disable the vessel throttle locked due to timewarping message notification. It sets the notification state for the "VesselThrottleLockedDueToTimewarpingMessage" notification.

SetManeuverNodeOutOfFuelState(bool state): This method is used to enable or disable the cannot place maneuver node while out of fuel message notification. It sets the notification state for the "CannotPlaceManeuverNodeWhileOutOfFuelMessage" notification.

SetGamePauseToggledState(bool state): This method is used to enable or disable the game pause toggled message notification. It sets the notification state for the "GamePauseToggledMessage" notification.

You can call these methods in your EnableNotifications method or any other relevant place in your code to enable or disable the respective notifications by passing the appropriate bool value (true for enable, false for disable).