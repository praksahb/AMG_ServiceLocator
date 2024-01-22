using ServiceLocator.Events;
using ServiceLocator.Map;
using ServiceLocator.Player;
using ServiceLocator.Sound;
using ServiceLocator.UI;
using ServiceLocator.Utilities;
using ServiceLocator.Wave;
using UnityEngine;

namespace Assets.Scripts.Main
{
    public class GameService : GenericMonoSingleton<GameService>
    {
        public PlayerService PlayerService { get; private set; }
        public SoundService SoundService { get; private set; }
        public UIService UIService => uIService;
        public EventService EventService { get; private set; }
        public MapService MapService { get; private set; }
        public WaveService WaveService { get; private set; }

        [SerializeField] private UIService uIService;

        [SerializeField] private PlayerScriptableObject playerScriptableObject;
        [SerializeField] private SoundScriptableObject soundScriptableObject;
        [SerializeField] private AudioSource audioEffects;
        [SerializeField] private AudioSource backgroundMusic;
        [SerializeField] private MapScriptableObject mapScriptableObject;
        [SerializeField] private WaveScriptableObject waveScriptableObject;

        private void Start()
        {
            PlayerService = new PlayerService(playerScriptableObject);
            SoundService = new SoundService(soundScriptableObject, backgroundMusic, audioEffects);
            EventService = new EventService();
            MapService = new MapService(mapScriptableObject);
            WaveService = new WaveService(waveScriptableObject);
        }

        private void Update()
        {
            PlayerService.Update();
        }
    }
}