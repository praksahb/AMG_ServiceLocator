using ServiceLocator.Wave.Bloon;
using UnityEngine;

namespace ServiceLocator.Player
{
    public class MonkeyView : MonoBehaviour
    {
        private MonkeyController controller;
        private CircleCollider2D rangeTriggerCollider;
        private Animator monkeyAnimator;
        [SerializeField] public SpriteRenderer RangeSpriteRenderer;

        private void Awake()
        {
            rangeTriggerCollider = GetComponent<CircleCollider2D>();
            monkeyAnimator = GetComponent<Animator>();
        }
        public void SetController(MonkeyController controller) => this.controller = controller;


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<BloonView>(out BloonView bloon))
            {
                controller.BloonEnteredRange(bloon.Controller);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.TryGetComponent<BloonView>(out BloonView bloon))
            {
                controller.BloonExitedRange(bloon.Controller);
            }
        }

        public void SetTriggerRadius(float radiusToSet)
        {
            if (rangeTriggerCollider != null)
                rangeTriggerCollider.radius = radiusToSet;

            RangeSpriteRenderer.transform.localScale = new Vector3(radiusToSet, radiusToSet, 1);
            MakeRangeVisible(false);
        }

        public void PlayAnimation(MonkeyAnimation animationToPlay) => monkeyAnimator.Play(animationToPlay.ToString(), 0);

        public void MakeRangeVisible(bool makeVisible) => RangeSpriteRenderer.color = makeVisible ? new Color(1, 1, 1, 0.25f) : new Color(1, 1, 1, 0);
    }

    public enum MonkeyAnimation
    {
        Idle,
        Shoot
    }
}
