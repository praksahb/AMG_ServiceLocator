using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ServiceLocator.UI
{
    public class MonkeyCellView : MonoBehaviour
    {
        private MonkeyCellController controller;

        [SerializeField] private MonkeyImageHandler monkeyImageHandler;
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI costText;
        [SerializeField] private Button lockButton;
        [SerializeField] private TextMeshProUGUI unlockCostText;

        public void SetController(MonkeyCellController controllerToSet) => controller = controllerToSet;

        public void ConfigureCellUI(Sprite spriteToSet, string nameToSet, int costToSet, bool isLocked, int unlockCost)
        {
            monkeyImageHandler.ConfigureImageHandler(spriteToSet, controller, isLocked);
            nameText.SetText(nameToSet);
            costText.SetText(costToSet.ToString());
            lockButton.gameObject.SetActive(isLocked);
            if (isLocked)
            {
                unlockCostText.SetText(unlockCost.ToString());
                lockButton.onClick.AddListener(UnlockButtonClicked);
            }
        }

        public void UnlockButtonClicked()
        {
            if (controller.TryUnlockingMonkey())
            {
                lockButton.gameObject.SetActive(false);
                monkeyImageHandler.UnlockImage();
            }
        }

    }
}