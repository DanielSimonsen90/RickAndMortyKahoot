﻿namespace RickAndMortyKahoot.Services.Score;

/// <summary>
/// Class that contains the constant score modifiers for answers
/// </summary>
public static class ScoreModifier
{
  public const int CORRECT_ANSWER = 100;
  public const int FAST_ANSWER = 150;
  public const int WRONG_ANSWER = 20;
  public const int SUBMITTED_ANSWER = 10;
  public const int TIMEDOUT_ANSWER = 0;
}
