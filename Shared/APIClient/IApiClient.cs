namespace Shared.APIClient
{
   public interface IApiClient
{
    Task<T> GetAsync<T>(string uri);
    Task<T> PostAsync<T>(string uri, object data);
    Task<T> PutAsync<T>(string uri, object data);
    Task DeleteAsync(string uri);
}
}
