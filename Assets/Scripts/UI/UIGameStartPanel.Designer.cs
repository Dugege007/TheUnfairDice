using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace TheUnfairDice
{
	// Generate Id:c0916120-0774-41f7-86ba-a39566f2ff2e
	public partial class UIGameStartPanel
	{
		public const string Name = "UIGameStartPanel";
		
		[SerializeField]
		public UnityEngine.UI.Button StartGameBtn;
		
		private UIGameStartPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			StartGameBtn = null;
			
			mData = null;
		}
		
		public UIGameStartPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIGameStartPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIGameStartPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
