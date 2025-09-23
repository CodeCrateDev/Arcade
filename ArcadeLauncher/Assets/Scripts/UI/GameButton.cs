using UnityEngine;
using UnityEngine.UI;
using ArcadeLauncher.Managers;
using ArcadeLauncher.Utils;
using ArcadeLauncher.Models;
using TMPro;

namespace ArcadeLauncher.UI
{
	[RequireComponent(typeof(Button))]
	public class GameButton : MonoBehaviour
	{
		private int id;
		private bool selected = false;

		[Header("Components")]
		[SerializeField] private TextMeshProUGUI gameName;
		[SerializeField] private Image border;
		[SerializeField] private Image gameImage;

		public void Setup(int manifestId)
		{
			id = manifestId;

			UpdateGame();
		}

		private void UpdateGame()
		{
			gameName.text = GameManager.Instance.GetGameManifest(id).gameName;

			gameImage.sprite = GameManager.Instance.GetGameImage(id);
		}

		public void ToggleSelect(bool value)
		{
			selected = value;

			if (selected)
			{
				border.color = Color.yellow;
				gameName.color = Color.yellow;
			}
			else
			{
				border.color = Color.white;
				gameName.color = Color.white;
			}
		}

		public void OnClick()
		{
			string gamePath = ExecutableHelper.GetGamePath(GameManager.Instance.GetGameManifest(id).gamePath);

			ExecutableHelper.LaunchGame(gamePath);
		}
	}
}