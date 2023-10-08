using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace TheUnfairDice
{
	// Generate Id:55a6e6f0-c3e0-4f39-afa5-d285ea90ccc6
	public partial class UIGamePanel
	{
		public const string Name = "UIGamePanel";
		
		[SerializeField]
		public UnityEngine.UI.Text PlayerHPText;
		[SerializeField]
		public UnityEngine.UI.Text FortressrHPText;
		[SerializeField]
		public UnityEngine.UI.Text HumanCountText;
		[SerializeField]
		public UnityEngine.UI.Text EnemyCountText;
		[SerializeField]
		public UnityEngine.UI.Text CurrentTimeText;
		
		private UIGamePanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			PlayerHPText = null;
			FortressrHPText = null;
			HumanCountText = null;
			EnemyCountText = null;
			CurrentTimeText = null;
			
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
