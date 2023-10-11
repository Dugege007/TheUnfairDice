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

                    // .Sequence() ����
                    ActionKit.Sequence()
                    .Lerp(0, 0.5f, 0.5f, p =>   // ����Ʈ�������
                    {
                        f.PositionY(positionY + p * 0.3f);
                        textComp.LocalScaleX(Mathf.Clamp01(p * 5f));
                        textComp.LocalScaleY(Mathf.Clamp01(p * 5f));
                    })
                    .Delay(0.5f)    // �ȴ� 0.5 ��
                    .Lerp(1f, 0, 0.3f, p => // ��͸��
                    {
                        textComp.ColorAlpha(p);

                    }, () =>    // Lerp ���֮��Ļص�
                    {
                        textTrans.parent.DestroyGameObjGracefully();
                        // Ŀǰͳһ���پͺã����������Ż�
                    })
                    .Start(textComp);   // ������������ Start �󶨸� textComp

                }).Show();
        }

        private void OnDestroy()
        {
            mDefault = null;
        }
    }
}
