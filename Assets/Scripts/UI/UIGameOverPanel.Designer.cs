using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace TheUnfairDice
{
	// Generate Id:625a3513-001b-44ad-9e1e-bffd56c8f427
	public partial class UIGameOverPanel
	{
		public const string Name = "UIGameOverPanel";
		
		[SerializeField]
		public UnityEngine.UI.Button BackToHomeBtn;
		
		private UIGameOverPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			BackToHomeBtn = null;
			
			mData = null;
		}
		
		public UIGameOverPanelData Data
		{
			get
			{
				return mData;
			}
		}
		
		UIGameOverPanelData mData
		{
			get
			{
				return mPrivateData ?? (mPrivateData = new UIGameOverPanelData());
			}
			set
			{
				mUIData = value;
				mPrivateData = value;
			}
		}
	}
}
