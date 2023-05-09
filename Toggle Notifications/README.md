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