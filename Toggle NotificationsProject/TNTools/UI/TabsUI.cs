using ToggleNotifications.Controller;
using ToggleNotifications.UI;
using UnityEngine;

namespace ToggleNotifications.TNTools.UI
{
    internal class TabsUI
    {
        internal List<IPageContent> Pages = new List<IPageContent>();
        private List<IPageContent> _filteredPages = new List<IPageContent>();
        private IPageContent CurrentPage;
        private List<float> TabsWidth = new List<float>();

        private bool tabButton(bool isCurrent, bool isActive, string txt)
        {
            GUIStyle style = isActive ? TNBaseStyle.TabActive : TNBaseStyle.TabNormal;
            return GUILayout.Toggle((isCurrent ? 1 : 0) != 0, txt, style, GUILayout.ExpandWidth(true));
        }

        internal int DrawTabs(int current, float maxWidth = 300f)
        {
            current = GeneralTools.ClampInt(current, 0, _filteredPages.Count - 1);
            GUILayout.BeginHorizontal();
            int num1 = current;
            if (TabsWidth.Count != _filteredPages.Count)
            {
                TabsWidth.Clear();
                for (int index = 0; index < _filteredPages.Count; ++index)
                {
                    IPageContent filteredPage = _filteredPages[index];
                    float minWidth;
                    TNBaseStyle.TabNormal.CalcMinMaxWidth(new GUIContent(filteredPage.Name, ""), out minWidth, out float _);
                    TabsWidth.Add(minWidth);
                }
            }
            float num2 = 0.0f;
            for (int index = 0; index < _filteredPages.Count; ++index)
            {
                IPageContent filteredPage = _filteredPages[index];
                float num3 = TabsWidth[index];
                if ((double)num2 > (double)maxWidth)
                {
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    num2 = 0.0f;
                }
                num2 += num3;
                bool isCurrent = current == index;
                if (tabButton(isCurrent, filteredPage.IsRunning, filteredPage.Name) && !isCurrent)
                    num1 = index;
            }
            GUILayout.EndHorizontal();
            UITools.Separator();
            return num1;
        }

        internal void Init()
        {
            Pages.Add(new BaseController());
        }

        internal void Update()
        {
            _filteredPages = new List<IPageContent>();
            for (int index = 0; index < Pages.Count; ++index)
            {
                if (Pages[index].IsActive)
                    _filteredPages.Add(Pages[index]);
            }
        }

        internal void OnGUI()
        {
            int mainTabIndex = TNBaseSettings.MainTabIndex;
            if (_filteredPages.Count == 0)
            {
                UITools.Error("NO active Tab tage !!!");
            }
            else
            {
                int index = GeneralTools.ClampInt(_filteredPages.Count != 1 ? DrawTabs(mainTabIndex) : 0, 0, _filteredPages.Count - 1);
                IPageContent filteredPage = _filteredPages[index];
                if (filteredPage != CurrentPage)
                {
                    CurrentPage.UIVisible = false;
                    CurrentPage = filteredPage;
                    CurrentPage.UIVisible = true;
                }
                TNBaseSettings.MainTabIndex = index;
                CurrentPage.OnGUI();
            }
        }
    }
}
