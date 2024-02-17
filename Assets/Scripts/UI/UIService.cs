using ServiceLocator.Events;
using ServiceLocator.Player;
using ServiceLocator.Wave;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace ServiceLocator.UI
{
    public class UIService : MonoBehaviour
    {
        // Dependencies:
        private WaveService waveService;
        private EventService eventService;

        [Header("Gameplay Panel")]
        [SerializeField] private GameObject gameplayPanel;
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private TextMeshProUGUI moneyText;
        [SerializeField] private TextMeshProUGUI waveProgressText;
        [SerializeField] private TextMeshProUGUI currentMapText;
        [SerializeField] private Button nextWaveButton;

        [Header("Level Selection Panel")]
        [SerializeField] private GameObject levelSelectionPanel;
        [SerializeField] private List<MapButton> mapButtons;
        [SerializeField] private GameObject lockedScreenDisplay;
        [SerializeField] private float lockMessageTime;
        private static int currentMapIndex = 0;
        private Coroutine newCoroutine;

        [Header("Monkey Selection UI")]
        private MonkeySelectionUIController monkeySelectionController;
        [SerializeField] private GameObject MonkeySelectionPanel;
        [SerializeField] private Transform cellContainer;
        [SerializeField] private MonkeyCellView monkeyCellPrefab;
        [SerializeField] private List<MonkeyCellScriptableObject> monkeyCellScriptableObjects;

        [Header("Game End Panel")]
        [SerializeField] private GameObject gameEndPanel;
        [SerializeField] private TextMeshProUGUI gameEndText;
        [SerializeField] private Button playAgainButton;
        [SerializeField] private Button quitButton;

        private void Start()
        {
            gameplayPanel.SetActive(false);
            gameEndPanel.SetActive(false);

            nextWaveButton.onClick.AddListener(OnNextWaveButton);
            quitButton.onClick.AddListener(OnQuitButtonClicked);
            playAgainButton.onClick.AddListener(OnPlayAgainButtonClicked);
        }

        public void Init(WaveService waveService, PlayerService playerService, EventService eventService)
        {
            this.waveService = waveService;
            this.eventService = eventService;

            InitializeMapSelectionUI(eventService);
            InitializeMonkeySelectionUI(playerService);
            SubscribeToEvents();
        }

        private void InitializeMapSelectionUI(EventService eventService)
        {
            levelSelectionPanel.SetActive(true);

            if (currentMapIndex >= mapButtons.Count)
            {
                Debug.Log("Errors");
                return;
            }
            for (int i = 0; i <= currentMapIndex; i++)
            {
                mapButtons[i].Init(eventService);

            }
            for (int i = 1; i < mapButtons.Count; i++)
            {
                mapButtons[i].LockButton(eventService);
            }
        }

        private void UnlockNextLevel()
        {
            if (++currentMapIndex >= mapButtons.Count) return;
            mapButtons[currentMapIndex].UnlockMap();
        }

        private void InitializeMonkeySelectionUI(PlayerService playerService)
        {
            monkeySelectionController = new MonkeySelectionUIController(playerService, cellContainer, monkeyCellPrefab, monkeyCellScriptableObjects);
            MonkeySelectionPanel.SetActive(false);
            monkeySelectionController.SetActive(false);
        }

        private void SubscribeToEvents()
        {
            eventService.OnMapSelected.AddListener(OnMapSelected);
            eventService.OnLockedMapSelected.AddListener(OnLockedMapSelected);
        }

        public void OnMapSelected(int mapID)
        {
            levelSelectionPanel.SetActive(false);
            gameplayPanel.SetActive(true);
            MonkeySelectionPanel.SetActive(true);
            monkeySelectionController.SetActive(true);
            currentMapText.SetText("Map: " + mapID);
        }

        private void OnLockedMapSelected(int mapID)
        {
            if (newCoroutine != null)
            {
                StopCoroutine(newCoroutine);
            }
            newCoroutine = StartCoroutine(DisplayLockedMessage(lockMessageTime));
        }

        private IEnumerator DisplayLockedMessage(float timer)
        {
            lockedScreenDisplay.SetActive(true);
            yield return new WaitForSeconds(timer);
            lockedScreenDisplay.SetActive(false);
        }

        private void OnNextWaveButton()
        {
            waveService.StarNextWave();
            SetNextWaveButton(false);
        }

        private void OnQuitButtonClicked() => Application.Quit();

        private void OnPlayAgainButtonClicked() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        public void SetNextWaveButton(bool setInteractable) => nextWaveButton.interactable = setInteractable;

        public void UpdateHealthUI(int healthToDisplay) => healthText.SetText(healthToDisplay.ToString());

        public void UpdateMoneyUI(int moneyToDisplay) => moneyText.SetText(moneyToDisplay.ToString());

        public void UpdateWaveProgressUI(int waveCompleted, int totalWaves) => waveProgressText.SetText(waveCompleted.ToString() + "/" + totalWaves.ToString());

        public void UpdateGameEndUI(bool hasWon)
        {
            gameplayPanel.SetActive(false);
            levelSelectionPanel.SetActive(false);
            gameEndPanel.SetActive(true);
            UnlockNextLevel();

            if (hasWon)
                gameEndText.SetText("You Won");
            else
                gameEndText.SetText("Game Over");
        }

    }
}