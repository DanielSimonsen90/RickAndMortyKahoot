namespace RickAndMortyKahoot.Hubs.Kahoot;

public partial class KahootHub
{
  /// <summary>
  /// Constant actions for hub
  /// </summary>
  /// <remarks>
  /// These actions should be used to call hub methods from client and must match the method names for actions
  /// </remarks>
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

  /// <summary>
  /// Constant events for hub
  /// </summary>
  /// <remarks>
  /// These events should be used to dispatch hub events from hub and must match the strings in client's implementation of the hub
  /// </remarks>
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
