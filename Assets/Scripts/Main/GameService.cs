using ServiceLocator.Events;
using ServiceLocator.Map;
using ServiceLocator.Player;
using ServiceLocator.Sound;
using ServiceLocator.UI;
using ServiceLocator.Wave;
using UnityEngine;

namespace ServiceLocator.Main
{
    public class GameService : MonoBehaviour
    {
        // Services:
        private EventService eventService;
        private MapService mapService;
        private WaveService waveService;
        private SoundService soundService;
        private PlayerService playerService;
        [SerializeField] private UIService uiService;


        // Scriptable Objects:
        [SerializeField] private MapScriptableObject mapScriptableObject;
        [SerializeField] private WaveScriptableObject waveScriptableObject;
        [SerializeField] private SoundScriptableObject soundScriptableObject;
        [SerializeField] private PlayerScriptableObject playerScriptableObject;

        // Scene References:
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private AudioSource bgMusicSource;

        private void Start()
        {
            InitializeServices();
            InjectDependencies();
        }

        private void InitializeServices()
        {
            eventService = new EventService();
            soundService = new SoundService(soundScriptableObject, sfxSource, bgMusicSource);
            mapService = new MapService(mapScriptableObject);
            playerService = new PlayerService(playerScriptableObject);
            waveService = new WaveService(waveScriptableObject);
        }

        private void InjectDependencies()
        {
            mapService.Init(eventService);
            uiService.Init(waveService, playerService, eventService);
            playerService.Init(mapService, uiService, soundService);
            waveService.Init(uiService, mapService, playerService, soundService, eventService);
        }

        private void Update()
        {
            playerService.Update();
            waveService.Update(Time.deltaTime);
        }
    }
}