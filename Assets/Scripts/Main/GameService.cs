using ServiceLocator.Player;
using ServiceLocator.Utilities;
using UnityEngine;

namespace Assets.Scripts.Main
{
    public class GameService : GenericMonoSingleton<GameService>
    {
        public PlayerService playerService { get; private set; }


        [SerializeField] public PlayerScriptableObject playerScriptableObject;


        private void Start()
        {
            playerService = new PlayerService(playerScriptableObject);
        }

        private void Update()
        {
            playerService.Update();
        }
    }
}