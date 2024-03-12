using TransformNewMailProvisionerData.Interfaces;
using TransformNewMailProvisionerData.Models.LegacySystem;
using TransformNewMailProvisionerData.Services.LegacySystem;

namespace TransformNewMailProvisionerData.Services.ProcessService
{
    public class MailProvisionDataStoreService : IMailProvisionDataStoreService
    {
        private readonly IDataService _service;
        private readonly IFileReaderService _fileReaderService;

        public MailProvisionDataStoreService(IDataService service, IFileReaderService fileReaderService)
        {
            _service = service;
            _fileReaderService = fileReaderService;
        }

        public async Task Process(string filePath)
        {


            if (!_fileReaderService.FileExists(filePath))
            {
                throw new Exception("File does not exist");
            }

            var rawList = _fileReaderService.ReadFile(filePath);

            var uniqueList = rawList
            .Where(c => c.Pid != 0)
            .GroupBy(c => c.Onyen, (key, g) => g.Last()).ToList();


            await _service.ImportRecords(uniqueList);

        }
    }
}