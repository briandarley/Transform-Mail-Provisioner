using TransformNewMailProvisionerData.Models.LegacySystem;

namespace TransformNewMailProvisionerData.Interfaces
{
    public interface IFileReaderService
    {
        bool FileExists(string path);
        List<ProvisioningJobModel> ReadFile(string path);
        void WriteFile(string filePath, List<ProvisioningJobModel> provisionJobs);
        Task<List<Models.LegacySystem.ProvisioningJobModel>> GetProvisionJobs(string filePath);
    }
}