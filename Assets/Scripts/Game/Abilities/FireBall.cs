using UnityEngine;
using QFramework;

namespace TheUnfairDice
{
    public partial class FireBall : ViewController
    {
        public float HolyFireSpeed = 8f;
        public float HolyFireDamage = 1f;

        private void Start()
        {
            SelfRigidbody2D.velocity = new Vector2(
                Random.Range(-1.0f, 1.0f),
                Random.Range(-1.0f, 1.0f) * Random.Range(HolyFireSpeed - 2, HolyFireSpeed + 2));

            HitHurtBox.OnTriggerEnter2DEvent(collider2D =>
            {
                HitHurtBox hurtBox = collider2D.GetComponent<HitHurtBox>();
                if (hurtBox != null)
                {
                    if (hurtBox.Owner.CompareTag("Enemy"))
                    {
                        if (Random.Range(0, 1.0f) < 0.5f)
                        {
                            // 击退效果
                            collider2D.attachedRigidbody.AddForce(
                                (collider2D.transform.position - transform.Position()).normalized * 500 +
                                (collider2D.transform.position - Player.Default.Position()).normalized * 1000);
                        }

                        Enemy enemy = hurtBox.Owner.GetComponent<Enemy>();
                        enemy.GetHurt(HolyFireDamage);
                    }
                }

            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // 获取法线
            Vector2 normal = collision.GetContact(0).normal;

            if (normal.x > normal.y)
            {
                SelfRigidbody2D.velocity =
                    new Vector2(SelfRigidbody2D.velocity.x,
                        Mathf.Sign(SelfRigidbody2D.velocity.y) *
                            Random.Range(0.8f, 1.2f) *
                            Random.Range(HolyFireSpeed - 2, HolyFireSpeed + 2));
                // Mathf.Sign() 只取正负符号

                // 随机自转
                SelfRigidbody2D.angularVelocity = Random.Range(-360, 360);
            }
            else
            {
                Rigidbody2D rb = SelfRigidbody2D;
                rb.velocity =
                    new Vector2(Mathf.Sign(rb.velocity.x) *
                            Random.Range(0.8f, 1.2f) *
                            Random.Range(HolyFireSpeed - 2, HolyFireSpeed + 2),
                        rb.velocity.y);
                // Mathf.Sign() 只取正负符号

                // 随机自转
                SelfRigidbody2D.angularVelocity = Random.Range(-360, 360);
            }
        }
    }
}
