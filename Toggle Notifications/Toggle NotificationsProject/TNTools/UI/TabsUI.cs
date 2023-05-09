using UnityEngine;

namespace ToggleNotifications.TNTools.UI
{
    public interface PageContent
    {
        // Name drawn in the Tab button
        string Name { get; }

        // if is isRunning, UI is drawn lighted
        bool isRunning { get; }

        // if isActive Tab is visible
        bool isActive { get; }

        // useful to know if current page is visible 
        bool UIVisible { get; set; }
        // Main Page UI called Here
        void onGUI();
    }
    public class SolarPanelTab : PageContent
    {
        private string name = "Solar Notification";
        private bool uiVisible = false;

        // Name drawn in the Tab button
        public string Name
        {
            get { return name; }
        }
        // if is isRunning, UI is drawn lighted
        public bool isRunning
        {
            get { return isActive; }
        }

        // if isActive Tab is visible
        public bool isActive
        {
            get { return isRunning; }
        }
        // useful to know if current page is visible (you can switch off not needed updates if not set)
        public bool UIVisible
        {
            get { return uiVisible; }
            set { uiVisible = value; }
        }
        public void onGUI()
        {
            // SolarPanelOptions-specific UI code here
        }
    }
    public class CommRangeTab : PageContent
    {
        private string name = "Comm Range Notification";
        private bool uiVisible = false;

        // Name drawn in the Tab button
        public string Name
        {
            get { return name; }
        }

        // if is isRunning, UI is drawn lighted
        public bool isRunning
        {
            get { return isRunning; }
        }

        // if isActive Tab is visible
        public bool isActive
        {
            get { return isActive; }
        }

        // useful to know if current page is visible (you can switch off not needed updates if not set)
        public bool UIVisible
        {
            get { return uiVisible; }
            set { uiVisible = value; }
        }

        // Main Page UI called Here
        public void onGUI()
        {
            // Implement your UI here
        }
    }
    public class ThrottleLockedTab : PageContent
    {
        private string name = "Throttle Locked Notification";
        private bool uiVisible = false;

        // Name drawn in the Tab button
        public string Name
        {
            get { return name; }
        }

        // if is isRunning, UI is drawn lighted
        public bool isRunning
        {
            get { return isRunning; }
        }

        // if isActive Tab is visible
        public bool isActive
        {
            get { return isActive; }
        }

        // useful to know if current page is visible (you can switch off not needed updates if not set)
        public bool UIVisible
        {
            get { return uiVisible; }
            set { uiVisible = value; }
        }

        // Main Page UI called Here
        public void onGUI()
        {
            // Implement your UI here
        }
    }
    public class ManeuverNodeTab : PageContent
    {
        private string name = "Maneuver Node Out of Fuel Notification";
        private bool uiVisible = false;

        // Name drawn in the Tab button
        public string Name
        {
            get { return name; }
        }

        // if is isRunning, UI is drawn lighted
        public bool isRunning
        {
            get { return isRunning; }
        }

        // if isActive Tab is visible
        public bool isActive
        {
            get { return isActive; }
        }

        // useful to know if current page is visible (you can switch off not needed updates if not set)
        public bool UIVisible
        {
            get { return uiVisible; }
            set { uiVisible = value; }
        }
        public void onGUI()
        {
            //
        }
    }
    public class GamePauseToggledTab : PageContent
    {
        private string name = "Game Pause Toggle Tab";
        private bool uiVisible = false;

        // Name drawn in the Tab button
        public string Name
        {
            get { return name; }
        }
        // if is isRunning, UI is drawn lighted
        public bool isRunning
        {
            get { return isActive; }
        }

        // if isActive Tab is visible
        public bool isActive
        {
            get { return isRunning; }
        }

        // useful to know if current page is visible (you can switch off not needed updates if not set)
        public bool UIVisible
        {
            get { return uiVisible; }
            set { uiVisible = value; }
        }
        public void onGUI()
        {
            //
        }
    }
    public class AllNotificationsTab : PageContent
    {
        private string name = "All Notifications Tab";
        private bool uiVisible = false;

        // Name drawn in the Tab button
        public string Name
        {
            get { return name; }
        }
        // if is isRunning, UI is drawn lighted
        public bool isRunning
        {
            get { return isActive; }
        }

        // if isActive Tab is visible
        public bool isActive
        {
            get { return isRunning; }
        }

        // useful to know if current page is visible (you can switch off not needed updates if not set)
        public bool UIVisible
        {
            get { return uiVisible; }
            set { uiVisible = value; }
        }
        public void onGUI()
        {
            //
        }
    }
    public class TabsUI
    {
        private List<string> tabNames = new List<string>();
        private Dictionary<string, string> tabContents = new Dictionary<string, string>();
        private Dictionary<string, BasePageContent> tabPages = new Dictionary<string, BasePageContent>();

        public List<PageContent> pages = new List<PageContent>();

        private List<PageContent> filteredPages = new List<PageContent>();

        PageContent CurrentPage = null;

        // must be called after adding pages
        private bool TabButton(bool isCurrent, bool isActive, string txt, string icon)
        {

            GUIStyle _style = isActive ? TNBaseStyle.TabActive : TNBaseStyle.TabNormal;
            if (icon == null)
                return GUILayout.Toggle(isCurrent, txt, _style, GUILayout.ExpandWidth(true));
            else
                return GUILayout.Toggle(isCurrent, icon, _style, GUILayout.ExpandWidth(true));
        }


        List<float> tabs_Width = new List<float>();


        public int DrawTabs(int current, float max_width = 300)
        {
            current = GeneralTools.ClampInt(current, 0, filteredPages.Count - 1);
            GUILayout.BeginHorizontal();

            int result = current;

            // compute sizes
            if (tabs_Width.Count != filteredPages.Count)
            {
                tabs_Width.Clear();
                for (int index = 0; index < filteredPages.Count; index++)
                {
                    var page = filteredPages[index];
                    float minWidth, maxWidth;
                    TNBaseStyle.TabNormal.CalcMinMaxWidth(new GUIContent(page.Name, ""), out minWidth, out maxWidth);
                    tabs_Width.Add(minWidth);
                }
            }
            float xPos = 0;

            for (int index = 0; index < filteredPages.Count; index++)
            {
                var page = filteredPages[index];

                float width = tabs_Width[index];

                if (xPos > max_width)
                {
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    xPos = 0;
                }
                xPos += width;
            }
            if (xPos < max_width * 0.7f)
            {
                GUILayout.FlexibleSpace();
            }
            GUILayout.EndHorizontal();

            UITools.Separator();
            return result;
        }

        public void AddTab(string name)
        {
            if (!tabNames.Contains(name))
            {
                tabNames.Add(name);
                tabContents.Add(name, "");
                tabPages.Add(name, new BasePageContent());
            }
        }

        public void AddContent(string name, string selection)
        {
            if (!tabNames.Contains(name))
            {
                tabNames.Add(name);
                tabContents.Add(name, "");
                tabPages.Add(name, new BasePageContent());
            }
            // add the selection content to the corresponding tab
            tabContents[name] += selection;
        }
        public void Init()
        {
            TabsUI tabsUI;
            CurrentPage = pages[TNBaseSettings.MainTabIndex];
            CurrentPage.UIVisible = true;

            // initialize the TabsUI instance and add the two new pages to it
            tabsUI = new TabsUI();
            tabsUI.pages.Add(new SolarPanelTab());
            tabsUI.pages.Add(new CommRangeTab());
            tabsUI.pages.Add(new ThrottleLockedTab());
            tabsUI.pages.Add(new ManeuverNodeTab());
            tabsUI.pages.Add(new GamePauseToggledTab());
            tabsUI.pages.Add(new AllNotificationsTab());
            tabsUI.Init();
        }
        // must be called to rebuild the filteredPages list 
        public void Update()
        {
            filteredPages = new List<PageContent>();
            for (int index = 0; index < pages.Count; index++)
            {
                if (pages[index].isActive)
                    filteredPages.Add(pages[index]);
            }
        }
        public void onGUI()
        {
            int _currentIndex = TNBaseSettings.MainTabIndex;

            if (filteredPages.Count == 0)
            {
                UITools.Error("NO active Tab tage !!!");
                return;
            }
            int result;
            if (filteredPages.Count == 1)
            {
                result = 0;
            }
            else
            {
                result = DrawTabs(_currentIndex);
            }

            result = GeneralTools.ClampInt(result, 0, filteredPages.Count - 1);
            PageContent page = filteredPages[result];

            if (page != CurrentPage)
            {
                CurrentPage.UIVisible = false;
                //KBaseSettings.MainTabIndex = result;
                //CurrentPage = filteredPages[result];
                CurrentPage = page;
                CurrentPage.UIVisible = true;
            }

            TNBaseSettings.MainTabIndex = result;

            foreach (var pageloop in filteredPages)
            {
                CurrentPage.onGUI();
            }
        }
    }
}

