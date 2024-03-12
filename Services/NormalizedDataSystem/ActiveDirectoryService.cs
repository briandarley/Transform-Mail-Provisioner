using TransformNewMailProvisionerData.Interfaces;
using TransformNewMailProvisionerData.Services.LegacySystem;
using UNC.Extensions.General;

namespace TransformNewMailProvisionerData.Services.NormalizedDataSystem
{
    public class ActiveDirectoryService : IActiveDirectoryService
    {

        private readonly IHttpClientFactory _httpClientFactory;
        public ActiveDirectoryService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<UNC.ActiveDirectory.Common.Entities.Entities.Entity> GetEntityByOnyen(string onyen)
        {

            var client = _httpClientFactory.CreateClient("LOCAL_AD");

            var criteria = new UNC.ActiveDirectory.Common.Criteria.Entities.EntitiesCriteria
            {
                SamAccountName = onyen
            };
            var request = await client.GetAsync($"entities?{criteria.ToQueryParams()}");

            var bodyRequest = await request.Content.ReadAsStringAsync();
            if (request.StatusCode == System.Net.HttpStatusCode.BadRequest) return null;

            var pgRequest = System.Text.Json.JsonSerializer.Deserialize<UNC.ActiveDirectory.Common.Pagination.PagedResponse<UNC.ActiveDirectory.Common.Entities.Entities.Entity>>(bodyRequest, new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            if (pgRequest is null) return null;
            if (pgRequest.TotalRecords != 1) return null;
            return pgRequest.Entities.Single();


        }
    }
}
