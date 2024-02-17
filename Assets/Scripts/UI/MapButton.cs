using ServiceLocator.Events;
using UnityEngine;
using UnityEngine.UI;

namespace ServiceLocator.UI
{
    public class MapButton : MonoBehaviour
    {
        [SerializeField] private int MapId;
        private EventService eventService;

        public void Init(EventService eventService)
        {
            this.eventService = eventService;
            GetComponent<Button>().onClick.AddListener(OnMapButtonClicked);
        }

        public void LockButton(EventService eventService)
        {
            this.eventService = eventService;
            GetComponent<Button>().onClick.AddListener(OnLockedMapButtonClicked);

        }

        public void UnlockMap()
        {
            Button button = GetComponent<Button>();
            button.onClick.RemoveListener(OnLockedMapButtonClicked);
            button.onClick.AddListener(OnMapButtonClicked);
        }

        // To Learn more about Events and Observer Pattern, check out the course list here: https://outscal.com/courses
        private void OnMapButtonClicked() => eventService.OnMapSelected.InvokeEvent(MapId);
        private void OnLockedMapButtonClicked() => eventService.OnLockedMapSelected.InvokeEvent(MapId);

    }
}