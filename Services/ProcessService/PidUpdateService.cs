using System.Collections.Concurrent;
using TransformNewMailProvisionerData.Interfaces;
using TransformNewMailProvisionerData.Services.LegacySystem;

namespace TransformNewMailProvisionerData.Services.ProcessService;

public class PidUpdateService : IPidUpdateService
{
    private readonly IProvisioningService _provisioningService;
    private readonly IFileReaderService _fileReaderService;
    private readonly IActiveDirectoryService _adService;

    public PidUpdateService(IActiveDirectoryService activeDirectoryService, IProvisioningService provisioningService, IFileReaderService fileReaderService)
    {

        _adService = activeDirectoryService;
        _provisioningService = provisioningService;
        _fileReaderService = fileReaderService;
    }


    public async Task Process(string filePath)
    {
        var provisionJobs = await _fileReaderService.GetProvisionJobs(filePath);
        //Remove any duplicates;
        CleanupProvisionJobService.RemoveDuplicates(provisionJobs);
        var entityList = new ConcurrentBag<UNC.ActiveDirectory.Common.Entities.Entities.Entity>();

        // foreach (var job in provisionJobs)
        // {
        //     var entity = await _adService.GetEntityByOnyen(job.Onyen);
        //     if (entity is null)
        //     {
        //         job.Active = false;
        //         Console.WriteLine($"NOT FOUND : {job.Onyen}");
        //         continue;
        //     }
        //     job.Pid = int.Parse(entity.EmployeeId);
        //     Console.WriteLine($"FOUND : {job.Onyen}");
        //     job.Active = true;
        // }
        int maxDegreeOfParallelism = 10;

        using var semaphoreSlim = new SemaphoreSlim(maxDegreeOfParallelism, maxDegreeOfParallelism);

        await Parallel.ForEachAsync(provisionJobs, async (job, cancelationTolen) =>
                {
                    await semaphoreSlim.WaitAsync();
                    try
                    {
                        var entity = await _adService.GetEntityByOnyen(job.Onyen);
                        if (entity is not null)
                        {
                            entityList.Add(entity);
                        }
                        else
                        {
                            job.Active = false;
                        }
                    }
                    finally
                    {
                        semaphoreSlim.Release();
                    }
                });


        entityList.Join(provisionJobs,
                        entity => entity.SamAccountName,
                        job => job.Onyen,
                        (entity, job) => new { entity, job })
                        .ToList()
                        .ForEach(x =>
                        {
                            x.job.Pid = int.Parse(x.entity.EmployeeId);
                            x.job.Active = true;
                        });

        //update the local file
        _fileReaderService.WriteFile(filePath, provisionJobs);
    }



}
