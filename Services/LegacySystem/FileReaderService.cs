using NameSpace.Models.LegacySystem;
using TransformNewMailProvisionerData.Interfaces;
using TransformNewMailProvisionerData.Models.LegacySystem;

namespace TransformNewMailProvisionerData.Services.LegacySystem;

public class FileReaderService : IFileReaderService
{
    private readonly IProvisioningService _provisioningService;

    public FileReaderService(IProvisioningService provisioningService)
    {
        _provisioningService = provisioningService;
    }

    public bool FileExists(string path)
    {
        return System.IO.File.Exists(path);
    }

    public List<ProvisioningJobModel> ReadFile(string path)
    {
        return System.Text.Json.JsonSerializer.Deserialize<List<ProvisioningJobModel>>(System.IO.File.ReadAllText(path), JsonOptions.Options);
    }

    public void WriteFile(string filePath, List<ProvisioningJobModel> provisionJobs)
    {
        var json = System.Text.Json.JsonSerializer.Serialize(provisionJobs);
        System.IO.File.WriteAllText(filePath, json);
    }

    public async Task<List<Models.LegacySystem.ProvisioningJobModel>> GetProvisionJobs(string filePath)
    {
        List<Models.LegacySystem.ProvisioningJobModel> provisionJobs = null;
        //see if the file exists

        if (!FileExists(filePath))
        {
            // Get all the provision jobs
            provisionJobs = await _provisioningService.GetAllProvisionJobs();
            //we wish to write file because this succer is big and we don't want to pull it twice
            WriteFile(filePath, provisionJobs);

            return provisionJobs;
        }

        provisionJobs = ReadFile(filePath);

        return provisionJobs;

    }
}