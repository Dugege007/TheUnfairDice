using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace TheUnfairDice
{
	// Generate Id:95647d3c-95d2-46ac-b34f-2a818da41d64
	public partial class UIGamePanel
	{
		public const string Name = "UIGamePanel";
		
		[SerializeField]
		public UnityEngine.UI.Text PlayerHPText;
		[SerializeField]
		public UnityEngine.UI.Text PlayerLevelText;
		[SerializeField]
		public UnityEngine.UI.Text CurrentExpText;
		[SerializeField]
		public UnityEngine.UI.Text FortressrHPText;
		[SerializeField]
		public UnityEngine.UI.Text HumanCountText;
		[SerializeField]
		public UnityEngine.UI.Text EnemyCountText;
		[SerializeField]
		public UnityEngine.UI.Text CurrentTimeText;
		[SerializeField]
		public UnityEngine.UI.Text GoldText;
		[SerializeField]
		public UnityEngine.UI.Text SpiritText;
		[SerializeField]
		public ExpUpgradePanel ExpUpgradePanel;
		
		private UIGamePanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			PlayerHPText = null;
			PlayerLevelText = null;
			CurrentExpText = null;
			FortressrHPText = null;
			HumanCountText = null;
			EnemyCountText = null;
			CurrentTimeText = null;
			GoldText = null;
			SpiritText = null;
			ExpUpgradePanel = null;
			
			mData = null;
		}
		
		public UIGamePanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIGamePanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIGamePanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
