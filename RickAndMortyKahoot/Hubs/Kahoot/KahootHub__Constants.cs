namespace RickAndMortyKahoot.Hubs.Kahoot;

public partial class KahootHub
{
  private static class Actions
  {
    public const string CONNECT = "Connect";
    public const string DISCONNECT = "Disconnect";

    public const string CREATE_GAME = "CreateGame";
    public const string START_GAME = "StartGame";
    public const string END_GAME = "EndGame";

    public const string NEXT_QUESTION = "NextQuestion";
    public const string SUBMIT_ANSWER = "SubmitAnswer";
    public const string END_ROUND = "EndRound";
  }
  private static class Events
  {
    public const string ERROR = "Error";  

    public const string USER_JOIN = "UserJoin";
    public const string USER_LEAVE = "UserLeave";

    public const string GAME_CREATE = "GameCreate";
    public const string GAME_START = "GameStart";
    public const string GAME_END = "GameEnd";

    public const string NEW_QUESTION = "NewQuestion";
    public const string ROUND_END = "RoundEnd";
    public const string AUTO_END_ROUND = "AutoEndRound";
  }
}
