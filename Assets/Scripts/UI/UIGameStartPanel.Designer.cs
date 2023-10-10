using System;
using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace TheUnfairDice
{
	// Generate Id:5fe9552c-6c70-43cb-a340-827b58159354
	public partial class UIGameStartPanel
	{
		public const string Name = "UIGameStartPanel";
		
		[SerializeField]
		public UnityEngine.UI.Button StartGameBtn;
		[SerializeField]
		public UnityEngine.UI.Button AboutBtn;
		[SerializeField]
		public UnityEngine.UI.Button QuitBtn;
		
		private UIGameStartPanelData mPrivateData = null;
		
		protected override void ClearUIComponents()
		{
			StartGameBtn = null;
			AboutBtn = null;
			QuitBtn = null;
			
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
