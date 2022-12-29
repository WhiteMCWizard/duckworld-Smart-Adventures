using SLAM.Engine;
using UnityEngine;

namespace SLAM.DuckQuiz
{
	public class DQCategoryView : View
	{
		[SerializeField]
		private UILabel lblCategoryName;

		[SerializeField]
		private UISprite sprtCategoryBackground;

		public void SetInfo(DQGameController.QuestionCategory category)
		{
			sprtCategoryBackground.spriteName = getSpriteNameForQuestionCategory(category);
			lblCategoryName.text = Localization.Get("DQ_" + getSpriteNameForQuestionCategory(category).ToUpper());
		}

		private string getSpriteNameForQuestionCategory(DQGameController.QuestionCategory questionCategory)
		{
			switch (questionCategory)
			{
			case DQGameController.QuestionCategory.Animals:
				return "categorie_Animals";
			case DQGameController.QuestionCategory.Disney:
				return "categorie_Disney";
			case DQGameController.QuestionCategory.Duck:
				return "categorie_Donald";
			case DQGameController.QuestionCategory.History:
				return "categorie_History";
			case DQGameController.QuestionCategory.Geography:
				return "categorie_Geo";
			default:
				return "categorie_Mix";
			}
		}
	}
}
