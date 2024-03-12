namespace TransformNewMailProvisionerData.Models.LegacySystem
{
	public class ProvisioningJobModel
	{
		public int Id { get; set; }
		public string JobType { get; set; }
		public int Pid { get; set; }
		public bool? Active { get; set; }
		public string Onyen { get; set; }
		public DateTime SubmittedDate { get; set; }
		public string MailboxType { get; set; }
		public string Status { get; set; }
		public string StatusDetail { get; set; }
		public DateTime? ScheduledDate { get; set; }
		public DateTime? CreatedDate { get; set; }
		public string SubmittedBy { get; set; }
		public string Requestor { get; set; }
		public DateTime? NotifiedDate { get; set; }
		public DateTime? CompletedDate { get; set; }
		public string EmailAddress { get; set; }
		public string Notes { get; set; }
	}
}