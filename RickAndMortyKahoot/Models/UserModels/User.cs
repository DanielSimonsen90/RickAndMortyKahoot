﻿namespace RickAndMortyKahoot.Models.UserModels;

public class User(string username)
{
  public Guid Id { get; set; } = Guid.NewGuid();
  public string Username { get; } = username;

  public Guid? GameId { get; set; }
}