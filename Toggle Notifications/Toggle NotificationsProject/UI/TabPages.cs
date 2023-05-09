using KSP.Game;
using SpaceWarp.API.Assets;
using ToggleNotifications.TNTools.UI;
using ToggleNotifications.UI;
using UnityEngine;

namespace ToggleNotifications;

public class BasePageContent : PageContent, SetSelection, SetOption
{
    protected NotificationEvents notificationEvents = new NotificationEvents();
    private void HandleAlertNotificationOpen()
    {
        foreach (var evt in notificationEvents.OnAlertNotificationOpen)
        {
            evt?.Post(MainUI.gameObject);
        }
    }
    private void HandleAlertNotificationClose()
    {
        foreach (var evt in notificationEvents.OnAlertNotificationClose)
        {
            evt?.Post(MainUI.gameObject);
        }
    }
    public BasePageContent()
    {
        this.MainUI = ToggleNotificationsUI.Instance;
        this.mainPlugin = ToggleNotificationsPlugin.Instance;

        // Register OnAlertNotificationOpen and OnAlertNotificationClose events
        this.MainUI.OnAlertNotificationOpen += HandleAlertNotificationOpen;
        this.MainUI.OnAlertNotificationClose += HandleAlertNotificationClose;
    }

    protected ToggleNotificationsUI MainUI;
    protected ToggleNotificationsPlugin mainPlugin;
    protected NotificationToggle currentState => MainUI.currentState;
    protected NotificationEvents OnAlertNotificationOpen => MainUI.RefreshState;
    public bool isRunning => false;
    bool ui_visible;
    public bool UIVisible { get => ui_visible; set => ui_visible = value; }

    public virtual bool isActive => throw new NotImplementedException();

    public virtual void onGUI()
    {
        throw new NotImplementedException();
    }
}
public class SolarPage : BasePageContent
{
    public override string Name => "Solar Notification";

    // readonly Texture2D tabIcon = AssetManager.GetAsset<Texture2D>($"{FlightPlanPlugin.Instance.SpaceWarpMetadata.ModID}/images/OwnshipManeuver_50v2.png");
    readonly Texture2D tabIcon = AssetManager.GetAsset<Texture2D>($"{ToggleNotificationsPlugin.Instance.SpaceWarpMetadata.ModID}/images/Capsule_v3_50.png");

    public override GUIContent Icon => new(tabIcon, "Solar");

    public virtual void onGUI()
    {
        throw new NotImplementedException();
    }
}
public class CommRangePage : BasePageContent
{
    //
}
public class ThrottlePage : BasePageContent
{
    //
}
public class NodePage : BasePageContent
{
    //
}
public class GamePausedPage : BasePageContent
{
    //
}
public class AllNotificationsPage : BasePageContent
{
    //
}

