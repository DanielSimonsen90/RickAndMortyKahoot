namespace RickAndMortyKahoot.Models.Question;

public class Question
{
  public Question(string title, string answer, string[] choices)
  {
    Title = title;
    Answer = answer;

    if (choices.Length != 4) throw new IndexOutOfRangeException("Choices must be of length 4");
    Choices = [.. choices];
  }

  public Guid Id { get; set; } = Guid.NewGuid();
  public string Title { get; }
  public string Answer { get; }
  public List<string> Choices { get; }

  public int ChoiceIndex => Choices.IndexOf(Answer);
}
