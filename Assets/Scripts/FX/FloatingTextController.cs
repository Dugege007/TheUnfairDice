using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace TheUnfairDice
{
    public partial class FloatingTextController : ViewController
    {
        private static FloatingTextController mDefault;

        private void Awake()
        {
            mDefault = this;
        }

        private void Start()
        {
            FloatingText.Hide();

        }

        public static void Play(Vector2 position, string text, bool critical = false)
        {
            mDefault.FloatingText.InstantiateWithParent(mDefault.transform)
                .PositionX(position.x)
                .PositionY(position.y)
                .Self(f =>
                {
                    Transform textTrans = f.transform.Find("Text");
                    Text textComp = textTrans.GetComponent<Text>();
                    textComp.text = text;

                    if (critical)
                        textComp.color = Color.red;

                    float positionY = position.y;

                    // .Sequence() 序列
                    ActionKit.Sequence()
                    .Lerp(0, 0.5f, 0.5f, p =>   // 向上飘动并变大
                    {
                        f.PositionY(positionY + p * 0.3f);
                        textComp.LocalScaleX(Mathf.Clamp01(p * 5f));
                        textComp.LocalScaleY(Mathf.Clamp01(p * 5f));
                    })
                    .Delay(0.5f)    // 等待 0.5 秒
                    .Lerp(1f, 0, 0.3f, p => // 变透明
                    {
                        textComp.ColorAlpha(p);

                    }, () =>    // Lerp 完成之后的回调
                    {
                        textTrans.parent.DestroyGameObjGracefully();
                        // 目前统一销毁就好，后面会进行优化
                    })
                    .Start(textComp);   // 将其生命周期 Start 绑定给 textComp

                }).Show();
        }

        private void OnDestroy()
        {
            mDefault = null;
        }
    }
}
