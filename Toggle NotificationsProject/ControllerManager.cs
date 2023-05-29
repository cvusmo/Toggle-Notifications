using ToggleNotifications.Controller;

namespace ToggleNotifications.Models
{
    public class ControllerManager
    {
        public List<BaseController> controllers = new List<BaseController>();

        public void AddController(BaseController controller) => controllers.Add(controller);

        public void onReset()
        {
            foreach (BaseController controller in controllers)
                controller.onReset();
        }

        public void UpdateControllers()
        {
            foreach (BaseController controller in controllers)
                controller.Update();
        }

        public void LateUpdateControllers()
        {
            foreach (BaseController controller in controllers)
                controller.LateUpdate();
        }

        public void FixedUpdateControllers()
        {
            foreach (BaseController controller in controllers)
                controller.FixedUpdate();
        }
    }
}
