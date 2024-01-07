using UnityEngine.Events;

namespace Assets.Scripts.Controllers
{
    public class GlobalEventsController
    {
        public UnityEvent<bool> MapPauseEvent = new UnityEvent<bool>();
    }
}
