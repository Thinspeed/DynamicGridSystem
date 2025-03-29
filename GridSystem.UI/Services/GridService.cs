using System.Net.Http.Json;
using GridSystem.UI.Extensions;
using GridSystem.UI.Models.Common;
using GridSystem.UI.Models.Grids;

namespace GridSystem.UI.Services;

public class GridService(HttpClient client)
{
    private static readonly string BaseUri = "/api/grid";
        
    public async Task<PagedList<Grid>?> GetPageAsync(int page, int pageSize)
    {
        string uri = BaseUri.CreatePagedUri(page, pageSize);

        return await client.GetFromJsonAsync<PagedList<Grid>>(uri);
    }

    public async Task<Grid?> GetByIdAsync(int id)
    {
        string uri = $"{BaseUri}/{id}";

        return await client.GetFromJsonAsync<Grid>(uri);
    }

    public async Task<int> CreateAsync(string name)
    {
        var body = new { name = name };
        
        HttpResponseMessage response = await client.PostAsJsonAsync(BaseUri, body);
        
        return response.IsSuccessStatusCode ? await response.Content.ReadFromJsonAsync<int>() : 0;
    }
}