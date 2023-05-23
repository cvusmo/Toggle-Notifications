using System;

namespace ToggleNotifications.Controller
{
    public class ButtonBase
    {
        public bool active = false;
        public Action action;

        public void _switch() => this.active = !this.active;

        public virtual void Run() => throw new NotImplementedException();
    }
}
