using System.Text;
using TransformNewMailProvisionerData.Interfaces;
using TransformNewMailProvisionerData.Models.LegacySystem;
using TransformNewMailProvisionerData.Services.LegacySystem;

namespace TransformNewMailProvisionerData.Services.NormalizedDataSystem
{
    public class DataService : IDataService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IFileReaderService _fileReaderService;

        public DataService(IHttpClientFactory httpClientFactory, IFileReaderService fileReaderService)
        {
            _httpClientFactory = httpClientFactory;
            _fileReaderService = fileReaderService;
        }

        public async Task ImportRecords(List<ProvisioningJobModel> entities)
        {
            var mappedEntities = MapToUserAccount(entities);
            await AddRecords(mappedEntities);
        }

        private List<UNC.DataAccessAPI.Common.Entities.MailProvisionDb.UserAccount> MapToUserAccount(List<ProvisioningJobModel> entities)
        {
            List<UNC.DataAccessAPI.Common.Entities.MailProvisionDb.ReplyToEmail> MapReplyToEmails(string rawEmails)
            {
                if (rawEmails == null) return new List<UNC.DataAccessAPI.Common.Entities.MailProvisionDb.ReplyToEmail>();

                var emails = rawEmails.Split(';');
                return emails.Select(x => new UNC.DataAccessAPI.Common.Entities.MailProvisionDb.ReplyToEmail
                {
                    Email = x
                }).ToList();
            }

            return entities.Select(x => new UNC.DataAccessAPI.Common.Entities.MailProvisionDb.UserAccount
            {
                Uid = x.Onyen,
                Pid = x.Pid,
                CreateDate = x.CreatedDate ?? DateTime.Now,
                CreateUser = x.SubmittedBy ?? x.Requestor ?? x.Onyen,
                ChangeDate = x.CompletedDate ?? DateTime.Now,
                ChangeUser = x.SubmittedBy ?? x.Requestor ?? x.Onyen,
                Notes = x.Notes,
                ProvisionedDate = x.CompletedDate,
                Status = x.Status,
                ReplyToEmails = MapReplyToEmails(x.EmailAddress),
                ProvisioningJobs = new List<UNC.DataAccessAPI.Common.Entities.MailProvisionDb.ProvisionJob>
                {
                    new UNC.DataAccessAPI.Common.Entities.MailProvisionDb.ProvisionJob
                    {

                    CreateDate = x.CreatedDate?? DateTime.Now,
                    CreateUser = x.SubmittedBy ?? x.Requestor ?? x.Onyen,
                    ChangeDate = x.CompletedDate?? DateTime.Now,
                    ChangeUser = x.SubmittedBy ?? x.Requestor ?? x.Onyen,
                    Notes = x.Notes,
                    Pid = x.Pid,
                    Requestor = x.Requestor,
                    CompletedDate = x.CompletedDate,
                    Status = x.Status,
                    StatusDetail = x.StatusDetail,
                    ProvisioningActions = new List<UNC.DataAccessAPI.Common.Entities.MailProvisionDb.ProvisionAction>
                    {
                        new UNC.DataAccessAPI.Common.Entities.MailProvisionDb.ProvisionAction
                        {
                            CreateDate = x.CreatedDate?? DateTime.Now,
                            CreateUser = x.SubmittedBy ?? x.Requestor ?? x.Onyen,
                            ChangeDate = x.CompletedDate?? DateTime.Now,
                            ChangeUser = x.SubmittedBy ?? x.Requestor ?? x.Onyen,
                            Notes = x.Notes,
                            ActionDate = x.CreatedDate?? DateTime.Now,
                            ActionType = x.Status,
                        }
                }
            }}
            }).ToList();
        }

        private async Task AddRecords(List<UNC.DataAccessAPI.Common.Entities.MailProvisionDb.UserAccount> entities)
        {
            try
            {
                foreach (var item in entities)
                {
                    var client = _httpClientFactory.CreateClient("LOCAL_DATA");
                    var request = new HttpRequestMessage(HttpMethod.Post, "mail-provision-db/user-accounts");
                    request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(item), Encoding.UTF8, "application/json");
                    var response = await client.SendAsync(request);
                    response.EnsureSuccessStatusCode();
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }


    }
}