using RickAndMorty.Net.Api.Models.Domain;
using RickAndMorty.Net.Api.Service;

namespace RickAndMortyKahoot.Services.RickAndMortyApi
{
  public class RickAndMortyKahootService(IRickAndMortyService ramApiService)
  {
    /// <summary>
    /// Get all data from the RickAndMortyApi
    /// </summary>
    /// <returns>Task with a tuple of characters, episodes and locations</returns>
    public async Task<(IEnumerable<Character>, IEnumerable<Episode>, IEnumerable<Location>)> GetAllData()
    {
      // Get all data from the RickAndMortyApi
      Task<IEnumerable<Character>> charactersReq = ramApiService.GetAllCharacters();
      Task<IEnumerable<Episode>> episodesReq = ramApiService.GetAllEpisodes();
      Task<IEnumerable<Location>> locationsReq = ramApiService.GetAllLocations();

      // Wait for all requests to finish
      await Task.WhenAll(charactersReq, episodesReq, locationsReq);

      // Return the results
      return (charactersReq.Result, episodesReq.Result, locationsReq.Result);
    }
  }
}
