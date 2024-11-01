﻿namespace RickAndMortyKahoot.Models.Questions;

/// <summary>
/// Represents a base question with a title, answer and choices
/// </summary>
/// <remarks>
/// This is used to create a <see cref="GameQuestion"/> with additional properties.
/// Choices must be of length 3 without answer or 4 with answer
/// </remarks>
public class Question
{
  public Question(string title, string answer, IEnumerable<string> choices)
  {
    Title = title;
    Answer = answer;

    // Check if choices are valid
    if (choices.Count() > 4) throw new IndexOutOfRangeException($"Choices cannot contain more items than 4 (is {choices.Count()})");
    if (choices.Count() < 4 && choices.Contains(answer)) 
      throw new IndexOutOfRangeException($"Choices must be of length 3 without answer or 4 with answer (is {choices.Count()})");
    if (choices.Count() == 4 && !choices.Contains(answer))
    {
      var choicesList = choices.ToList();
      choicesList.RemoveAt(0);
      choices = choicesList;
    }
    if (choices.Count() == 3) choices = choices.Append(answer);
    Choices = [.. choices];
  }

  public Guid Id { get; set; } = Guid.NewGuid();
  public string Title { get; }
  public string Answer { get; }
  public List<string> Choices { get; internal set; }

  public int AnswerIndex
  {
    get
    {
      // If index is found, return it
      int index = Choices.IndexOf(Answer);
      if (index != -1) return index;

      // If index fails, replace a random choice with the answer and return the index
      int randIndex = new Random().Next(Choices.Count);
      Choices[randIndex] = Answer;
      return randIndex;
    }
  }
}
