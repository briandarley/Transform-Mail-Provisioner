//add namespace Name
using NameSpace.Models.LegacySystem;
using TransformNewMailProvisionerData.Models.LegacySystem;
using UNC.Extensions.General;

namespace TransformNewMailProvisionerData.Services.LegacySystem
{
    /// <summary>
    /// Service to interact with the legacy system, pulls from Provisioning Data Store
    /// </summary>
    public class ProvisioningService : IProvisioningService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ProvisioningService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<ProvisioningJobModel>> GetAllProvisionJobs()
        {

            var client = _httpClientFactory.CreateClient("LOCAL_AD");

            var response = new List<ProvisioningJobModel>();
            var criteria = new ProvisionCriteria
            {
                PageSize = 100,
                Index = 0
            };

            PagedResponse<ProvisioningJobModel> pgResponse;

            do
            {
                if ((criteria.Index % 10) == 0)
                {
                    Console.WriteLine($"Total Records Read: {response.Count()}");
                }

                var request = await client.GetAsync($"provision-jobs?{criteria.ToQueryParams()}");
                request.EnsureSuccessStatusCode();

                var bodyRequest = await request.Content.ReadAsStringAsync();

                if (bodyRequest is null)
                {
                    throw new Exception("No data returned from the request");
                }


                pgResponse = System.Text.Json.JsonSerializer.Deserialize<PagedResponse<ProvisioningJobModel>>(bodyRequest, JsonOptions.Options);

                if (pgResponse?.Entities != null)
                {
                    response.AddRange(pgResponse.Entities);
                }

                criteria.Index++;

            } while (pgResponse != null && pgResponse.Entities.Any());

            return response;
        }

    }
}
