// See https://aka.ms/new-console-template for more information
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TransformNewMailProvisionerData.Services.ProcessService;
using TransformNewMailProvisionerData.WorkTasks;


// IConfiguration configuration = new ConfigurationBuilder()
//           .SetBasePath(Directory.GetCurrentDirectory())
//           .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
//           .AddEnvironmentVariables()
//           // .AddCommandLine(args) // To add command-line parameters
//           .Build();

// var serviceProvider = new ServiceCollection();
//serviceProvider.ConfigureServices(configuration);

//serviceProvider.BuildServiceProvider();

CreateHostBuilder(null).Build().Run();

//const string filePath = @"f:\temp\provision-list-2024-03-07.dat";




//var adService = new ActiveDirectoryService();
//Console.WriteLine(adService.GetEntityByOnyen("bdarley").GetAwaiter().GetResult());
static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) =>
        {

            services.ConfigureServices(hostContext.Configuration);

        });