using UnityEngine;
using System.Collections.Generic;
using DocumentFormat.OpenXml.Wordprocessing;

namespace ToggleNotifications.TNTools.UI
{
    public class TabsUI
    {
        private List<string> tabNames = new List<string>();
        private Dictionary<string, string> tabContents = new Dictionary<string, string>();
        private Dictionary<string, BasePageContent> tabPages = new Dictionary<string, BasePageContent>();
        public static NotificationToggle currentState;

        public List<IPageContent> pages = new List<IPageContent>();

        private List<IPageContent> filteredPages = new List<IPageContent>();

        private IPageContent currentPage;

        private List<float> tabsWidth = new List<float>();

        public bool IsActive { get; internal set; }
        public bool IsVisible { get; internal set; }

        private bool tabButton(bool isCurrent, bool isActive, string txt, GUIContent icon)
        {
            GUIStyle style = isActive ? TNBaseStyle.TabActive : TNBaseStyle.TabNormal;
            return icon == null ? GUILayout.Toggle(isCurrent, txt, style, GUILayout.ExpandWidth(true)) : GUILayout.Toggle(isCurrent, icon, style, GUILayout.ExpandWidth(true));
        }

        public int DrawTabs(int current, float maxWidth = 300f)
        {
            current = Mathf.Clamp(current, 0, filteredPages.Count - 1);
            GUILayout.BeginHorizontal();
            int num1 = current;
            if (tabsWidth.Count != filteredPages.Count)
            {
                tabsWidth.Clear();
                for (int index = 0; index < filteredPages.Count; ++index)
                {
                    IPageContent filteredPage = filteredPages[index];
                    float minWidth;
                    TNBaseStyle.TabNormal.CalcMinMaxWidth(new GUIContent(filteredPage.Name, ""), out minWidth, out _);
                    tabsWidth.Add(minWidth);
                }
            }
            float num2 = 0.0f;
            for (int index = 0; index < filteredPages.Count; ++index)
            {
                IPageContent filteredPage = filteredPages[index];
                float num3 = tabsWidth[index];
                if (num2 > maxWidth)
                {
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    num2 = 0.0f;
                }
                num2 += num3;
                bool isCurrent = current == index;
                if (tabButton(isCurrent, filteredPage.IsActive, filteredPage.Name, filteredPage.Icon) && !isCurrent)
                    num1 = index;
            }
            GUILayout.EndHorizontal();
            UITools.Separator();
            return num1;
        }

        public void Init()
        {
            currentPage = pages[TNBaseSettings.MainTabIndex];
        }

        public void Update()
        {
            filteredPages.Clear();
            for (int index = 0; index < pages.Count; ++index)
            {
                if (pages[index].IsActive)
                    filteredPages.Add(pages[index]);
            }
        }

        public void OnGUI()
        {
            int mainTabIndex = TNBaseSettings.MainTabIndex;
            if (filteredPages.Count == 0)
            {
                UITools.Error("NO active Tab tag found!!!");
            }
            else
            {
                int index = Mathf.Clamp(filteredPages.Count != 1 ? DrawTabs(mainTabIndex) : 0, 0, filteredPages.Count - 1);
                IPageContent filteredPage = filteredPages[index];
                if (filteredPage != currentPage)
                {
                    currentPage = filteredPage;
                }
                TNBaseSettings.MainTabIndex = index;
                currentPage.OnGUI();
            }
        }
    }
}