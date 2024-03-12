using TransformNewMailProvisionerData.Interfaces;

namespace TransformNewMailProvisionerData.WorkTasks
{
    public class WorkerTask : IWorkerTask
    {
        private readonly IPidUpdateService _pidUpdateService;
        private readonly IMailProvisionDataStoreService _mailProvisionDataStoreService;


        public WorkerTask(IPidUpdateService pidUpdateService, IMailProvisionDataStoreService mailProvisionDataStoreService)
        {
            _pidUpdateService = pidUpdateService;
            _mailProvisionDataStoreService = mailProvisionDataStoreService;

        }
        public async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Your background task logic here
                Console.WriteLine($"Worker running at: {DateTimeOffset.Now}");
                //await Task.Delay(1000, stoppingToken); // Wait for 1 second

                //await _pidUpdateService.Process(@"f:\temp\provision-list-2024-03-07.dat");
                await _mailProvisionDataStoreService.Process(@"f:\temp\provision-list-2024-03-07.dat");
                break;
                //await Task.Delay(1000, stoppingToken); // Wait for 1 second
            }
        }

    }
}