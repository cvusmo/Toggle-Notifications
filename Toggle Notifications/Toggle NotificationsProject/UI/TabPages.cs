using ToggleNotifications.TNTools.UI;

namespace ToggleNotifications;

public class BasePageContent : PageContent
{
    public BasePageContent()
    {
        this.main_ui = ToggleNotificationsUI.Instance;
        this.plugin = ToggleNotificationsPlugin.Instance;
    }
    protected ToggleNotificationsUI main_ui;
    protected ToggleNotificationsPlugin plugin;


    //protected PatchedConicsOrbit orbit => main_ui.orbit;
    //protected CelestialBodyComponent referenceBody => main_ui.referenceBody;

    public virtual string Name => throw new NotImplementedException();

    public bool isRunning => false;


    bool ui_visible;
    public bool UIVisible { get => ui_visible; set => ui_visible = value; }

    public virtual bool isActive => throw new NotImplementedException();

    public virtual void onGUI()
    {
        throw new NotImplementedException();
    }
}

public class TargetPage : BasePageContent
{
    public override string Name => "Target";

    public override bool isActive
    {
        get => plugin.currentTarget != null  // If the activeVessel and the currentTarget are both orbiting the same body
            && plugin.currentTarget.Orbit != null // No maneuvers relative to a star
            && plugin.currentTarget.Orbit.referenceBody.Name == referenceBody.Name;
    }

    public override void onGUI()
    {
        TNStyles.DrawSectionHeader("Notifications");

        SolarPanelOption.Instance.SolarTypeSelectionGUI();

        main_ui.DrawToggleButton("Match Planes", ManeuverType.matchPlane);
        main_ui.DrawToggleButton("Hohmann Transfer", ManeuverType.hohmannXfer);
        main_ui.DrawToggleButton("Course Correction", ManeuverType.courseCorrection);

        if (plugin.experimental.Value)
        {
            FPSettings.interceptT = main_ui.DrawToggleButtonWithTextField("Intercept", ManeuverType.interceptTgt, FPSettings.interceptT, "s");
            main_ui.DrawToggleButton("Match Velocity", ManeuverType.matchVelocity);
        }
    }
}
public class SolarPage : BasePageContent
{
    //
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

