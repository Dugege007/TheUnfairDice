/****************************************************************************
 * 2023.10 MSI
 ****************************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace TheUnfairDice
{
    public partial class ExpUpgradePanel : UIElement, IController
    {
        public Sprite[] ButtonSprites = new Sprite[0];

        private void Awake()
        {
            TempleteBtn.Hide();

            ExpUpgradeSystem expUpgradeSystem = this.GetSystem<ExpUpgradeSystem>();

            for (int i = 0; i < expUpgradeSystem.Items.Count; i++)
            {
                TempleteBtn.InstantiateWithParent(UpgradeRoot)
                    .Self(self =>
                    {
                        ExpUpgradeItem itemCache = expUpgradeSystem.Items[i];
                        Button selfCache = self;

                        self.onClick.AddListener(() =>
                        {
                            Time.timeScale = 1.0f;
                            itemCache.Upgrade();
                            this.Hide();
                        });

                        for (int j = 0; j < ConfigManager.Default.AbilityConfigs.Length; j++)
                        {
                            if (i == j)
                            {
                                selfCache.GetComponent<Image>().sprite = ButtonSprites[j];
                                break;
                            }
                        }
                        selfCache.Hide();

                        itemCache.Visible.RegisterWithInitValue(visible =>
                        {
                            if (visible)
                            {
                                selfCache.GetComponentInChildren<Text>().text = itemCache.Description;
                                selfCache.Show();
                            }
                            else
                            {
                                selfCache.Hide();
                            }

                        }).UnRegisterWhenGameObjectDestroyed(selfCache);

                        itemCache.CurrentLevel.RegisterWithInitValue(lv =>
                        {
                            selfCache.GetComponentInChildren<Text>().text = itemCache.Description;

                        }).UnRegisterWhenGameObjectDestroyed(selfCache);
                    });
            }
        }

        private void Start()
        {
            SkipBtn.onClick.AddListener(() =>
            {
                Time.timeScale = 1.0f;
                this.Hide();
            });
        }

        protected override void OnBeforeDestroy()
        {
        }

        public IArchitecture GetArchitecture()
        {
            return Global.Interface;
        }
    }
}