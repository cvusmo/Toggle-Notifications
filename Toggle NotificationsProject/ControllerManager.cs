using ToggleNotifications.Controller;

namespace ToggleNotifications.Models
{
    public class ControllerManager
    {
        public List<BaseController> controllers = new List<BaseController>();

        public void AddController(BaseController controller) => this.controllers.Add(controller);

        public void onReset()
        {
            foreach (BaseController controller in this.controllers)
                controller.onReset();
        }

        public void UpdateControllers()
        {
            foreach (BaseController controller in this.controllers)
                controller.Update();
        }

        public void LateUpdateControllers()
        {
            foreach (BaseController controller in this.controllers)
                controller.LateUpdate();
        }

        public void FixedUpdateControllers()
        {
            foreach (BaseController controller in this.controllers)
                controller.FixedUpdate();
        }
    }
}
