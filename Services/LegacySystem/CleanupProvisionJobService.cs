using TransformNewMailProvisionerData.Models.LegacySystem;

namespace TransformNewMailProvisionerData.Services.LegacySystem
{
    public class CleanupProvisionJobService
    {
        /// <summary>
        /// Remove duplicates from the list of ProvisioningJobModel, PID must be unique
        /// </summary>
        /// <param name="entities"></param>
        public static void RemoveDuplicates(List<ProvisioningJobModel> entities)
        {
            entities = entities.GroupBy(c => c.Onyen, (key, entities) => new { Pid = key, Entity = entities.Last() }).Select(c => c.Entity).ToList();
        }
    }
}
