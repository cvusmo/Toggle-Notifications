namespace ToggleNotifications.TNTools.UI
{
    public class UIController : BlankMonoBehaviour
    {
        public BasePageContent[] pages; // An array of all the pages you want to display
        public string[] pageNames; // An array of names for each page, to be displayed in the UI
        public int currentPageIndex; // The index of the current page

        // Constructor
        public UIController(ToggleNotificationsPlugin mainPlugin)
        {
            pages = new BasePageContent[]
            {
                new SolarPage(mainPlugin),
                new CommRangePage(mainPlugin),
                new ThrottlePage(mainPlugin),
                new NodePage(mainPlugin),
                new GamePausedPage(mainPlugin),
            };
        }

        // Use this method to switch to a different page
        public void SwitchToPage(int index)
        {
            // Disable the current page
            //pages[currentPageIndex].gameObject.SetActive(false);

            // Set the new page index and enable the new page
            currentPageIndex = index;
            //pages[currentPageIndex].gameObject.SetActive(true);
        }

        // Call this method to switch to the next page
        public void NextPage()
        {
            int nextPageIndex = currentPageIndex + 1;
            if (nextPageIndex >= pages.Length)
            {
                nextPageIndex = 0;
            }
            SwitchToPage(nextPageIndex);
        }

        // Call this method to switch to the previous page
        public void PreviousPage()
        {
            int previousPageIndex = currentPageIndex - 1;
            if (previousPageIndex < 0)
            {
                previousPageIndex = pages.Length - 1;
            }
            SwitchToPage(previousPageIndex);
        }
    }
}