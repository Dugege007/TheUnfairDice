/****************************************************************************
 * 2023.10 MSI
 ****************************************************************************/

using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace TheUnfairDice
{
	public partial class ExpUpgradePanel
	{
		[SerializeField] public UnityEngine.UI.Button TempleteBtn;
		[SerializeField] public RectTransform UpgradeRoot;
		[SerializeField] public UnityEngine.UI.Button SkipBtn;

		public void Clear()
		{
			TempleteBtn = null;
			UpgradeRoot = null;
			SkipBtn = null;
		}

		public override string ComponentName
		{
			get { return "ExpUpgradePanel";}
		}
	}
}
