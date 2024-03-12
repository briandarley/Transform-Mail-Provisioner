namespace TransformNewMailProvisionerData.Models.LegacySystem
{
    public class ProvisionCriteria : BasePagingCriteria<ProvisioningJobModel>
    {
        public int? ProvisionId { get; set; }
        public string Onyen { get; set; }
        public DateTime? SubmittedFromDate { get; set; }
        public DateTime? SubmittedThruDate { get; set; }
        public string Status { get; set; }


    }

}