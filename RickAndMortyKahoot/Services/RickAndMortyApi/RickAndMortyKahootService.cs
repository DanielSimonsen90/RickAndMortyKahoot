using RickAndMorty.Net.Api.Models.Domain;
using RickAndMorty.Net.Api.Service;

namespace RickAndMortyKahoot.Services.RickAndMortyApi
{
  public class RickAndMortyKahootService(IRickAndMortyService ramApiService)
  {
    //public IRickAndMortyService RamApiService { get; } = ramApiService;

    public async Task<(IEnumerable<Character>, IEnumerable<Episode>, IEnumerable<Location>)> GetAllData()
    {
      Task<IEnumerable<Character>> charactersReq = ramApiService.GetAllCharacters();
      Task<IEnumerable<Episode>> episodesReq = ramApiService.GetAllEpisodes();
      Task<IEnumerable<Location>> locationsReq = ramApiService.GetAllLocations();

      await Task.WhenAll(charactersReq, episodesReq, locationsReq);

      return (charactersReq.Result, episodesReq.Result, locationsReq.Result);
    }
  }
}
