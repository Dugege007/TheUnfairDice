using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace TheUnfairDice
{
	// Generate Id:b0f336b0-7906-4850-b573-58ff12a6b28a
	public partial class UIGamePassPanel
	{
		public const string Name = "UIGamePassPanel";
		
		[SerializeField]
		public UnityEngine.UI.Button BackToHomeBtn;
		
		private UIGamePassPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			BackToHomeBtn = null;
			
			mData = null;
		}
		
		public UIGamePassPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIGamePassPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIGamePassPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
