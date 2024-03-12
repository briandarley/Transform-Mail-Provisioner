using TransformNewMailProvisionerData.Models.LegacySystem;

public interface IProvisioningService
{
    Task<List<ProvisioningJobModel>> GetAllProvisionJobs();
}