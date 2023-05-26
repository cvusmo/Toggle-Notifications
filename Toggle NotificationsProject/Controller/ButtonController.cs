namespace ToggleNotifications.Controller
{
    public class ButtonController : BaseController
    {
        public List<ButtonBase> buttons = new List<ButtonBase>();

        private protected void Run()
        {
            foreach (ButtonBase buttonBase in this.buttons.Where<ButtonBase>((Func<ButtonBase, bool>)(button => button.active)))
                buttonBase.Run();
        }
    }
}
