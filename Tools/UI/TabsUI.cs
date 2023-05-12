using ToggleNotifications.TNTools.UI;
using ToggleNotifications.TNTools;
using UnityEngine;
using KSP.Messages;

namespace ToggleNotifications.TNTools.UI;

public interface IPageContent
{
    MessageCenterMessage ConvertToMessageCenterMessage(NotificationToggle currentState);
    string Name { get; }
    GUIContent Icon { get; }
    bool IsRunning { get; }
    bool IsActive { get; }
    bool UIVisible { get; set; }

    void OnGUI();
}

public class TabsUI
{
    private List<string> tabNames = new List<string>();
    private Dictionary<string, string> tabContents = new Dictionary<string, string>();
    private Dictionary<string, BasePageContent> tabPages = new Dictionary<string, BasePageContent>();
    public static NotificationToggle currentState;

    public List<IPageContent> pages = new List<IPageContent>();

    private List<IPageContent> filteredPages = new List<IPageContent>();

    IPageContent CurrentPage = null;

    // must be called after adding pages
    private bool tabButton(bool isCurrent, bool IsActive, string txt, GUIContent icon)
    {
        GUIStyle style = IsActive ? TNBaseStyle.TabActive : TNBaseStyle.TabNormal;
        if (icon == null)
            return GUILayout.Toggle(isCurrent, txt, style, GUILayout.ExpandWidth(true));
        else
            return GUILayout.Toggle(isCurrent, icon, style, GUILayout.ExpandWidth(true));
    }

    List<float> TabsWidth = new();

    public int DrawTabs(int current, float maxWidth = 300)
    {
        current = GeneralTools.ClampInt(current, 0, filteredPages.Count - 1);
        GUILayout.BeginHorizontal();

        int result = current;

        // compute sizes
        if (TabsWidth.Count != filteredPages.Count)
        {
            TabsWidth.Clear();
            for (int index = 0; index < filteredPages.Count; index++)
            {
                var page = filteredPages[index];
                TNBaseStyle.TabNormal.CalcMinMaxWidth(new GUIContent(page.Name, ""), out float _minWidth, out _);
                TabsWidth.Add(_minWidth);
            }
        }
        float _xPos = 0;

        for (int index = 0; index < filteredPages.Count; index++)
        {
            IPageContent page = filteredPages[index];

            float _width = TabsWidth[index];

            if (_xPos > maxWidth)
            {
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                _xPos = 0;
            }
            _xPos += _width;

            bool _isCurrent = current == index;
            if (tabButton(_isCurrent, page.IsRunning, page.Name, page.Icon))
            {
                if (!_isCurrent)

                    result = index;
            }
        }

        /*  if (_xPos < _maxWidth * 0.7f)
          {
              GUILayout.FlexibleSpace();
          }*/
        GUILayout.EndHorizontal();

        UITools.Separator();
        return result;
    }
    public void AddTab(string name, ToggleNotificationsPlugin plugin)
    {
        if (!tabContents.ContainsKey(name))
        {
            tabContents.Add(name, "");
            tabPages.Add(name, new SolarPage(plugin));
        }
    }

    public void AddContent(string name, string selection, ToggleNotificationsPlugin plugin)
    {
        if (!tabContents.ContainsKey(name))
        {
            tabContents.Add(name, "");
            tabPages.Add(name, new SolarPage(plugin));
        }
        // add the selection content to the corresponding tab
        tabContents[name] += selection;
    }

    public void Init()
    {
        CurrentPage = pages[TNBaseSettings.MainTabIndex];
        CurrentPage.UIVisible = true;
    }

    // must be called to rebuild the filteredPages list 
    public void Update()
    {
        filteredPages = new List<IPageContent>();
        for (int index = 0; index < pages.Count; index++)
        {
            if (pages[index].IsActive)
                filteredPages.Add(pages[index]);
        }
    }

    public void OnGUI()
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
        IPageContent page = filteredPages[result];

        if (page != CurrentPage)
        {
            CurrentPage.UIVisible = false;
            //TNBaseSettings.MainTabIndex = result;
            //CurrentPage = filteredPages[result];
            CurrentPage = page;
            CurrentPage.UIVisible = true;
        }

        TNBaseSettings.MainTabIndex = result;

        CurrentPage.OnGUI();
    }
}