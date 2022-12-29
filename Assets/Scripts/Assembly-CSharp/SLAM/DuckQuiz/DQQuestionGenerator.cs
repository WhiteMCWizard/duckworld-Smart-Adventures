using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using SLAM.Slinq;
using UnityEngine;

namespace SLAM.DuckQuiz
{
	public class DQQuestionGenerator : MonoBehaviour
	{
		[Serializable]
		public struct DQQuestionsAsset
		{
			[CompilerGenerated]
			private sealed class _003CparseFlagQuestion_003Ec__Iterator78 : IEnumerator, IDisposable, IEnumerable, IEnumerator<DQQuestion>, IEnumerable<DQQuestion>
			{
				internal List<DQFlagQuestion> _003Cquestions_003E__0;

				internal string[] _003CallowedContinents_003E__1;

				internal int _003Ci_003E__2;

				internal string[] lines;

				internal string[] _003CquestionParts_003E__3;

				internal DQQuestionGenerator gen;

				internal IEnumerable<DQAnswer> _003CallAnswers_003E__4;

				internal int _003Cj_003E__5;

				internal List<DQAnswer> _003Canswers_003E__6;

				internal int _0024PC;

				internal DQQuestion _0024current;

				internal string[] _003C_0024_003Elines;

				internal DQQuestionGenerator _003C_0024_003Egen;

				internal DQQuestionsAsset _003C_003Ef__this;

				public static Func<DQFlagQuestion, DQAnswer> _003C_003Ef__am_0024cacheE;

				DQQuestion IEnumerator<DQQuestion>.Current
				{
					[DebuggerHidden]
					get
					{
						return _0024current;
					}
				}

				object IEnumerator.Current
				{
					[DebuggerHidden]
					get
					{
						return _0024current;
					}
				}

				[DebuggerHidden]
				IEnumerator IEnumerable.GetEnumerator()
				{
					return System_002ECollections_002EGeneric_002EIEnumerable_003CSLAM_002EDuckQuiz_002EDQQuestion_003E_002EGetEnumerator();
				}

				[DebuggerHidden]
				IEnumerator<DQQuestion> IEnumerable<DQQuestion>.GetEnumerator()
				{
					if (Interlocked.CompareExchange(ref _0024PC, 0, -2) == -2)
					{
						return this;
					}
					return new _003CparseFlagQuestion_003Ec__Iterator78
					{
						_003C_003Ef__this = _003C_003Ef__this,
						lines = _003C_0024_003Elines,
						gen = _003C_0024_003Egen
					};
				}

				public bool MoveNext()
				{
					//Discarded unreachable code: IL_02bf
					uint num = (uint)_0024PC;
					_0024PC = -1;
					switch (num)
					{
					case 0u:
					{
						_003Cquestions_003E__0 = new List<DQFlagQuestion>();
						_003CallowedContinents_003E__1 = _003C_003Ef__this.getAllowedContinentsPerDifficulty(_003C_003Ef__this.Difficulty);
						for (_003Ci_003E__2 = 1; _003Ci_003E__2 < lines.Length; _003Ci_003E__2++)
						{
							_003CquestionParts_003E__3 = lines[_003Ci_003E__2].Split(new char[1] { ';' }, StringSplitOptions.RemoveEmptyEntries);
							if (_003CquestionParts_003E__3.Length < 4)
							{
								UnityEngine.Debug.LogError("Line #" + _003Ci_003E__2 + " in " + _003C_003Ef__this.textAsset.name + " is invalid, not enough answers?");
							}
							else if (_003CallowedContinents_003E__1.Contains(_003CquestionParts_003E__3[4]))
							{
								_003Cquestions_003E__0.Add(new DQFlagQuestion
								{
									Text = _003CquestionParts_003E__3[1],
									Continent = _003CquestionParts_003E__3[4],
									FlagName = Path.GetFileNameWithoutExtension(_003CquestionParts_003E__3[3]),
									Flag = gen.GetFlagTexture(Path.GetFileNameWithoutExtension(_003CquestionParts_003E__3[3])),
									Category = _003C_003Ef__this.Category,
									Difficulty = _003C_003Ef__this.Difficulty,
									Answers = new DQAnswer[0]
								});
							}
						}
						_003Cquestions_003E__0 = _003Cquestions_003E__0.Shuffle();
						List<DQFlagQuestion> collection = _003Cquestions_003E__0;
						if (_003C_003Ef__am_0024cacheE == null)
						{
							_003C_003Ef__am_0024cacheE = _003C_003Em__56;
						}
						_003CallAnswers_003E__4 = collection.Select(_003C_003Ef__am_0024cacheE);
						_003Cj_003E__5 = 0;
						goto IL_029e;
					}
					case 1u:
						{
							_003Cj_003E__5++;
							goto IL_029e;
						}
						IL_029e:
						if (_003Cj_003E__5 < _003Cquestions_003E__0.Count)
						{
							_003Canswers_003E__6 = _003CallAnswers_003E__4.Where(_003C_003Em__57).Take(3).ToList();
							_003Canswers_003E__6.Add(new DQAnswer
							{
								Text = _003Cquestions_003E__0[_003Cj_003E__5].FlagName,
								Correct = true
							});
							_003Cquestions_003E__0[_003Cj_003E__5].Answers = _003Canswers_003E__6.Shuffle().ToArray();
							_0024current = _003Cquestions_003E__0[_003Cj_003E__5];
							_0024PC = 1;
							return true;
						}
						_0024PC = -1;
						break;
					}
					return false;
				}

				[DebuggerHidden]
				public void Dispose()
				{
					_0024PC = -1;
				}

				[DebuggerHidden]
				public void Reset()
				{
					throw new NotSupportedException();
				}

				public static DQAnswer _003C_003Em__56(DQFlagQuestion q)
				{
					return new DQAnswer
					{
						Text = q.FlagName,
						Correct = false
					};
				}

				internal bool _003C_003Em__57(DQAnswer a)
				{
					return a.Text != _003Cquestions_003E__0[_003Cj_003E__5].FlagName;
				}
			}

			[CompilerGenerated]
			private sealed class _003CparseCapitalQuestion_003Ec__Iterator79 : IEnumerator, IDisposable, IEnumerable, IEnumerator<DQQuestion>, IEnumerable<DQQuestion>
			{
				internal List<DQCapitalQuestion> _003Cquestions_003E__0;

				internal string[] _003CallowedContinents_003E__1;

				internal int _003Ci_003E__2;

				internal string[] lines;

				internal string[] _003CquestionParts_003E__3;

				internal IEnumerable<DQBonusAnswer> _003CallAnswers_003E__4;

				internal int _003Cj_003E__5;

				internal List<DQBonusAnswer> _003Canswers_003E__6;

				internal int _0024PC;

				internal DQQuestion _0024current;

				internal string[] _003C_0024_003Elines;

				internal DQQuestionsAsset _003C_003Ef__this;

				public static Func<DQCapitalQuestion, DQBonusAnswer> _003C_003Ef__am_0024cacheC;

				DQQuestion IEnumerator<DQQuestion>.Current
				{
					[DebuggerHidden]
					get
					{
						return _0024current;
					}
				}

				object IEnumerator.Current
				{
					[DebuggerHidden]
					get
					{
						return _0024current;
					}
				}

				[DebuggerHidden]
				IEnumerator IEnumerable.GetEnumerator()
				{
					return System_002ECollections_002EGeneric_002EIEnumerable_003CSLAM_002EDuckQuiz_002EDQQuestion_003E_002EGetEnumerator();
				}

				[DebuggerHidden]
				IEnumerator<DQQuestion> IEnumerable<DQQuestion>.GetEnumerator()
				{
					if (Interlocked.CompareExchange(ref _0024PC, 0, -2) == -2)
					{
						return this;
					}
					return new _003CparseCapitalQuestion_003Ec__Iterator79
					{
						_003C_003Ef__this = _003C_003Ef__this,
						lines = _003C_0024_003Elines
					};
				}

				public bool MoveNext()
				{
					//Discarded unreachable code: IL_02ae
					uint num = (uint)_0024PC;
					_0024PC = -1;
					switch (num)
					{
					case 0u:
					{
						_003Cquestions_003E__0 = new List<DQCapitalQuestion>();
						_003CallowedContinents_003E__1 = _003C_003Ef__this.getAllowedContinentsPerDifficulty(_003C_003Ef__this.Difficulty);
						for (_003Ci_003E__2 = 1; _003Ci_003E__2 < lines.Length; _003Ci_003E__2++)
						{
							_003CquestionParts_003E__3 = lines[_003Ci_003E__2].Split(new char[1] { ';' }, StringSplitOptions.RemoveEmptyEntries);
							if (_003CquestionParts_003E__3.Length < 4)
							{
								UnityEngine.Debug.LogError("Line #" + _003Ci_003E__2 + " in " + _003C_003Ef__this.textAsset.name + " is invalid, not enough answers?");
							}
							else if (_003CallowedContinents_003E__1.Contains(_003CquestionParts_003E__3[4]))
							{
								_003Cquestions_003E__0.Add(new DQCapitalQuestion
								{
									Text = string.Format("{0} {1}", _003CquestionParts_003E__3[1], _003CquestionParts_003E__3[2]),
									Continent = _003CquestionParts_003E__3[4],
									Capital = _003CquestionParts_003E__3[3],
									Category = _003C_003Ef__this.Category,
									Difficulty = _003C_003Ef__this.Difficulty,
									Answers = new DQAnswer[0]
								});
							}
						}
						_003Cquestions_003E__0 = _003Cquestions_003E__0.Shuffle();
						List<DQCapitalQuestion> collection = _003Cquestions_003E__0;
						if (_003C_003Ef__am_0024cacheC == null)
						{
							_003C_003Ef__am_0024cacheC = _003C_003Em__58;
						}
						_003CallAnswers_003E__4 = collection.Select(_003C_003Ef__am_0024cacheC);
						_003Cj_003E__5 = 0;
						goto IL_028d;
					}
					case 1u:
						{
							_003Cj_003E__5++;
							goto IL_028d;
						}
						IL_028d:
						if (_003Cj_003E__5 < _003Cquestions_003E__0.Count)
						{
							_003Canswers_003E__6 = _003CallAnswers_003E__4.Where(_003C_003Em__59).Take(3).ToList();
							_003Canswers_003E__6.Add(new DQBonusAnswer
							{
								Text = _003Cquestions_003E__0[_003Cj_003E__5].Capital,
								Correct = true
							});
							_003Cquestions_003E__0[_003Cj_003E__5].Answers = _003Canswers_003E__6.Shuffle().ToArray();
							_0024current = _003Cquestions_003E__0[_003Cj_003E__5];
							_0024PC = 1;
							return true;
						}
						_0024PC = -1;
						break;
					}
					return false;
				}

				[DebuggerHidden]
				public void Dispose()
				{
					_0024PC = -1;
				}

				[DebuggerHidden]
				public void Reset()
				{
					throw new NotSupportedException();
				}

				public static DQBonusAnswer _003C_003Em__58(DQCapitalQuestion q)
				{
					return new DQBonusAnswer
					{
						Text = q.Capital,
						Correct = false,
						Continent = q.Continent
					};
				}

				internal bool _003C_003Em__59(DQBonusAnswer a)
				{
					return a.Text != _003Cquestions_003E__0[_003Cj_003E__5].Capital && a.Continent == _003Cquestions_003E__0[_003Cj_003E__5].Continent;
				}
			}

			private const char CSV_SEPERATOR_CHARACTER = ';';

			[SerializeField]
			private TextAsset textAsset;

			[Popup(new string[] { "Dutch", "English" })]
			[SerializeField]
			private string languageKey;

			[SerializeField]
			private DQGameController.QuestionCategory category;

			[SerializeField]
			private DQGameController.QuestionDifficulty difficulty;

			public DQGameController.QuestionCategory Category
			{
				get
				{
					return category;
				}
			}

			public DQGameController.QuestionDifficulty Difficulty
			{
				get
				{
					return difficulty;
				}
			}

			public string LanguageKey
			{
				get
				{
					return languageKey;
				}
			}

			public IEnumerable<DQQuestion> GetQuestions(DQQuestionGenerator gen)
			{
				string[] lines = textAsset.text.Split(new string[3] { "\r\n", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);
				switch (Category)
				{
				default:
					return parseNormalQuestions(lines);
				case DQGameController.QuestionCategory.Flags:
					return parseFlagQuestion(lines, gen);
				case DQGameController.QuestionCategory.Capitals:
					return parseCapitalQuestion(lines);
				}
			}

			private IEnumerable<DQQuestion> parseNormalQuestions(string[] lines)
			{
				for (int i = 1; i < lines.Length; i++)
				{
					string[] questionParts = lines[i].Split(new char[1] { ';' }, StringSplitOptions.RemoveEmptyEntries);
					if (questionParts.Length < 3)
					{
						UnityEngine.Debug.Log("Line #" + i + " in " + textAsset.name + " is invalid, not enough answers?");
						yield return null;
					}
					DQAnswer[] answers = new DQAnswer[questionParts.Length - 2];
					for (int j = 0; j < answers.Length; j++)
					{
						answers[j] = new DQAnswer
						{
							Correct = (j == 0),
							Text = questionParts[j + 2]
						};
					}
					yield return new DQQuestion
					{
						Text = questionParts[1],
						Answers = answers.Shuffle(),
						Category = Category,
						Difficulty = Difficulty
					};
				}
			}

			private IEnumerable<DQQuestion> parseFlagQuestion(string[] lines, DQQuestionGenerator gen)
			{
				List<DQFlagQuestion> questions = new List<DQFlagQuestion>();
				string[] allowedContinents = getAllowedContinentsPerDifficulty(Difficulty);
				for (int i = 1; i < lines.Length; i++)
				{
					string[] questionParts = lines[i].Split(new char[1] { ';' }, StringSplitOptions.RemoveEmptyEntries);
					if (questionParts.Length < 4)
					{
						UnityEngine.Debug.LogError("Line #" + i + " in " + textAsset.name + " is invalid, not enough answers?");
					}
					else if (allowedContinents.Contains(questionParts[4]))
					{
						questions.Add(new DQFlagQuestion
						{
							Text = questionParts[1],
							Continent = questionParts[4],
							FlagName = Path.GetFileNameWithoutExtension(questionParts[3]),
							Flag = gen.GetFlagTexture(Path.GetFileNameWithoutExtension(questionParts[3])),
							Category = Category,
							Difficulty = Difficulty,
							Answers = new DQAnswer[0]
						});
					}
				}
				questions = questions.Shuffle();
				List<DQFlagQuestion> collection = questions;
				if (_003CparseFlagQuestion_003Ec__Iterator78._003C_003Ef__am_0024cacheE == null)
				{
					_003CparseFlagQuestion_003Ec__Iterator78._003C_003Ef__am_0024cacheE = _003CparseFlagQuestion_003Ec__Iterator78._003C_003Em__56;
				}
				IEnumerable<DQAnswer> allAnswers = collection.Select(_003CparseFlagQuestion_003Ec__Iterator78._003C_003Ef__am_0024cacheE);
				for (int j = 0; j < questions.Count; j++)
				{
					List<DQAnswer> answers = allAnswers.Where(((_003CparseFlagQuestion_003Ec__Iterator78)(object)this)._003C_003Em__57).Take(3).ToList();
					answers.Add(new DQAnswer
					{
						Text = questions[j].FlagName,
						Correct = true
					});
					questions[j].Answers = answers.Shuffle().ToArray();
					yield return questions[j];
				}
			}

			private IEnumerable<DQQuestion> parseCapitalQuestion(string[] lines)
			{
				List<DQCapitalQuestion> questions = new List<DQCapitalQuestion>();
				string[] allowedContinents = getAllowedContinentsPerDifficulty(Difficulty);
				for (int i = 1; i < lines.Length; i++)
				{
					string[] questionParts = lines[i].Split(new char[1] { ';' }, StringSplitOptions.RemoveEmptyEntries);
					if (questionParts.Length < 4)
					{
						UnityEngine.Debug.LogError("Line #" + i + " in " + textAsset.name + " is invalid, not enough answers?");
					}
					else if (allowedContinents.Contains(questionParts[4]))
					{
						questions.Add(new DQCapitalQuestion
						{
							Text = string.Format("{0} {1}", questionParts[1], questionParts[2]),
							Continent = questionParts[4],
							Capital = questionParts[3],
							Category = Category,
							Difficulty = Difficulty,
							Answers = new DQAnswer[0]
						});
					}
				}
				questions = questions.Shuffle();
				List<DQCapitalQuestion> collection = questions;
				if (_003CparseCapitalQuestion_003Ec__Iterator79._003C_003Ef__am_0024cacheC == null)
				{
					_003CparseCapitalQuestion_003Ec__Iterator79._003C_003Ef__am_0024cacheC = _003CparseCapitalQuestion_003Ec__Iterator79._003C_003Em__58;
				}
				IEnumerable<DQBonusAnswer> allAnswers = collection.Select(_003CparseCapitalQuestion_003Ec__Iterator79._003C_003Ef__am_0024cacheC);
				for (int j = 0; j < questions.Count; j++)
				{
					List<DQBonusAnswer> answers = allAnswers.Where(((_003CparseCapitalQuestion_003Ec__Iterator79)(object)this)._003C_003Em__59).Take(3).ToList();
					answers.Add(new DQBonusAnswer
					{
						Text = questions[j].Capital,
						Correct = true
					});
					questions[j].Answers = answers.Shuffle().ToArray();
					yield return questions[j];
				}
			}

			private string[] getAllowedContinentsPerDifficulty(DQGameController.QuestionDifficulty diff)
			{
				switch (diff)
				{
				default:
					return new string[1] { "Europa" };
				case DQGameController.QuestionDifficulty.Medium:
					return new string[2] { "Noord- en Midden-Amerika", "Zuid-Amerika" };
				case DQGameController.QuestionDifficulty.Hard:
					return new string[2] { "Afrika", "Azië" };
				}
			}
		}

		[CompilerGenerated]
		private sealed class _003CGetQuestions_003Ec__AnonStorey167
		{
			internal DQGameController.QuestionCategory category;

			internal DQGameController.QuestionDifficulty difficulty;

			internal string languageKey;

			internal DQQuestionGenerator _003C_003Ef__this;

			internal bool _003C_003Em__53(DQQuestionsAsset asset)
			{
				return (asset.Category & category) == asset.Category && (asset.Difficulty & difficulty) == asset.Difficulty && asset.LanguageKey == languageKey;
			}

			internal IEnumerable<DQQuestion> _003C_003Em__54(DQQuestionsAsset d)
			{
				return d.GetQuestions(_003C_003Ef__this);
			}
		}

		[CompilerGenerated]
		private sealed class _003CGetFlagTexture_003Ec__AnonStorey168
		{
			internal string flagName;

			internal bool _003C_003Em__55(Texture2D f)
			{
				return f.name == flagName;
			}
		}

		[SerializeField]
		private Texture2D[] flags;

		[SerializeField]
		private DQQuestionsAsset[] assets;

		public IEnumerable<DQQuestion> GetQuestions(string languageKey, DQGameController.QuestionCategory category, DQGameController.QuestionDifficulty difficulty)
		{
			_003CGetQuestions_003Ec__AnonStorey167 _003CGetQuestions_003Ec__AnonStorey = new _003CGetQuestions_003Ec__AnonStorey167();
			_003CGetQuestions_003Ec__AnonStorey.category = category;
			_003CGetQuestions_003Ec__AnonStorey.difficulty = difficulty;
			_003CGetQuestions_003Ec__AnonStorey.languageKey = languageKey;
			_003CGetQuestions_003Ec__AnonStorey._003C_003Ef__this = this;
			return assets.Where(_003CGetQuestions_003Ec__AnonStorey._003C_003Em__53).SelectMany(_003CGetQuestions_003Ec__AnonStorey._003C_003Em__54).ToList()
				.Shuffle();
		}

		public Texture2D GetFlagTexture(string flagName)
		{
			_003CGetFlagTexture_003Ec__AnonStorey168 _003CGetFlagTexture_003Ec__AnonStorey = new _003CGetFlagTexture_003Ec__AnonStorey168();
			_003CGetFlagTexture_003Ec__AnonStorey.flagName = flagName;
			return flags.FirstOrDefault(_003CGetFlagTexture_003Ec__AnonStorey._003C_003Em__55);
		}
	}
}
