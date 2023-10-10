using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace TheUnfairDice
{
	// Generate Id:c12199d6-66a0-4b60-be3b-1350fff8de47
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
