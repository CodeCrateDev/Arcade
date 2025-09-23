using ArcadeLauncher.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace ArcadeLauncher.UI
{
	public class GameMenu : MonoBehaviour
	{
		[Header("References")]
		[SerializeField] private List<GameButton> buttons = new List<GameButton>();

		private int currentPage = 0;
		private int maxPage;
		private int currentSelected = -1;

		private const int GAMES_PER_PAGE = 6;

		private void Awake()
		{
			GameManager.Instance.FinishedLoadingGames += OnFinishedLoadingGames;
		}

		private void OnFinishedLoadingGames()
		{
			maxPage = GameManager.Instance.GetGameCount() % buttons.Count;

			UpdatePage();

			SelectGame(0);
		}

		public void SelectGame(int index)
		{
			if (currentSelected >= 0)
			{
				buttons[currentSelected].ToggleSelect(false);
				buttons[index].ToggleSelect(true);

				currentSelected = index;
			}
			else
			{
				buttons[index].ToggleSelect(true);

				currentSelected = index;
			}
		}

		private void UpdatePage()
		{
			for (int i = 0; i < buttons.Count; i++)
			{
				if (i + (currentPage * GAMES_PER_PAGE) < GameManager.Instance.GetGameCount())
				{
					buttons[i].Setup(i + (currentPage * GAMES_PER_PAGE));
					buttons[i].gameObject.SetActive(true);
				}
				else
				{
					buttons[i].gameObject.SetActive(false);
				}
			}
		}

		public void NextPage()
		{

		}

		public void PreviousPage()
		{

		}
	}

}