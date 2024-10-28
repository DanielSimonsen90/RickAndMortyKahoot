namespace RickAndMortyKahoot.Models.Questions;

public class Question
{
  public Question(string title, string answer, IEnumerable<string> choices)
  {
    Title = title;
    Answer = answer;

    if ((choices.Count() < 4 && !choices.Contains(answer)) || choices.Count() > 4) throw new IndexOutOfRangeException("Choices must be of length 3 without answer or 4 with answer");
    if (choices.Count() == 3) choices = choices.Append(answer);
    Choices = [.. choices];
  }

  public Guid Id { get; set; } = Guid.NewGuid();
  public string Title { get; }
  public string Answer { get; }
  public List<string> Choices { get; }

  public int AnswerIndex => Choices.IndexOf(Answer);
}
