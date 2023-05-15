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
            // Toggle the mainPlugin.pauseToggleState to control the notification
            mainPlugin.pauseToggleState = !mainPlugin.pauseToggleState;

            // Check the current state of the GamePausedPage
        }

        // Use this method to switch to a different page
        public void SwitchToPage(int index)
        {
            // Deactivate the current page

            // Set the new page index and activate the new page
            currentPageIndex = index;
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
