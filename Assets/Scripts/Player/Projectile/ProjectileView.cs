using ServiceLocator.Wave.Bloon;
using UnityEngine;

namespace ServiceLocator.Player.Projectile
{
    public class ProjectileView : MonoBehaviour
    {
        private ProjectileController controller;
        private SpriteRenderer spriteRenderer;
        private CircleCollider2D circleCollider;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            circleCollider = GetComponent<CircleCollider2D>();
        }
        public void SetController(ProjectileController controller) => this.controller = controller;

        private void Update()
        {
            if (ProjectileOutOfBounds())
                controller.ResetProjectile();

            controller?.UpdateProjectileMotion();
        }

        private bool ProjectileOutOfBounds() => !spriteRenderer.isVisible;

        public void SetSprite(Sprite spriteToSet) => spriteRenderer.sprite = spriteToSet;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out BloonView component))
            {
                controller.OnHitBloon(component.Controller);

                if (controller.ProjectileType == ProjectileType.Canon)
                {
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, circleCollider.radius);
                    if (colliders.Length > 0)
                    {
                        foreach (Collider2D collider in colliders)
                        {
                            if (collider.TryGetComponent(out BloonView bloonView))
                            {
                                controller.OnHitBloon(bloonView.Controller);
                            }
                        }
                    }
                }
            }

        }



    }
}