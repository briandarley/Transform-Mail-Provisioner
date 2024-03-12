
using TransformNewMailProvisionerData.Models.LegacySystem;

namespace TransformNewMailProvisionerData.Interfaces
{
    public interface IDataService
    {
        Task ImportRecords(List<ProvisioningJobModel> entities);
    }
}