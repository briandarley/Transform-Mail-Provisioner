
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TransformNewMailProvisionerData.Interfaces;
using TransformNewMailProvisionerData.Pocos;
using TransformNewMailProvisionerData.Services.LegacySystem;
using TransformNewMailProvisionerData.Services.NormalizedDataSystem;
using TransformNewMailProvisionerData.Services.ProcessService;
using TransformNewMailProvisionerData.WorkTasks;

public static class DependencyInjection
{
    public static ServiceProvider ServiceProvider { get; private set; }

    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient();
        ConfigurHttpClient(services, configuration);
        services.AddSingleton<ITokenService, TokenService>();
        services.AddTransient<BearerTokenHandler>();
        services.AddTransient<IProvisioningService, ProvisioningService>();
        services.AddTransient<IActiveDirectoryService, ActiveDirectoryService>();
        services.AddTransient<IFileReaderService, FileReaderService>();
        services.AddTransient<IPidUpdateService, PidUpdateService>();
        services.AddTransient<IMailProvisionDataStoreService, MailProvisionDataStoreService>();
        services.AddTransient<IDataService, DataService>();

        services.AddSingleton<IWorkerTask, WorkerTask>();
        services.AddHostedService<Worker>();


    }

    private static void ConfigurHttpClient(IServiceCollection services, IConfiguration configuration)
    {
        var idpConfigurations = configuration.GetSection("IdpConfigurations").Get<List<IdpConfiguration>>();

        //Retrieve configuration from user secrets
        idpConfigurations.Single(c => c.Name == "LOCAL_IDP").ClientId = configuration["LOCAL_IDP:CLIENT_ID"];
        idpConfigurations.Single(c => c.Name == "LOCAL_IDP").ClientSecret = configuration["LOCAL_IDP:CLIENT_SECRET"];

        idpConfigurations.Single(c => c.Name == "UAT_IDP").ClientId = configuration["UAT_IDP:CLIENT_ID"];
        idpConfigurations.Single(c => c.Name == "UAT_IDP").ClientSecret = configuration["UAT_IDP:CLIENT_SECRET"];


        services.AddHttpClient("LOCAL_AD", client =>
       {
           client.BaseAddress = new Uri("https://localhost:5503/v1/");
           client.Timeout = new TimeSpan(0, 1, 0);
       })
       .AddHttpMessageHandler(provider => new BearerTokenHandler(new TokenService("LOCAL_IDP", idpConfigurations)))
       .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
       {
           ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
       });

        services.AddHttpClient("LOCAL_DATA", client =>
        {
            client.BaseAddress = new Uri("https://localhost:15501/v1/");
            client.Timeout = new TimeSpan(0, 1, 0);
        })
        .AddHttpMessageHandler(provider => new BearerTokenHandler(new TokenService("LOCAL_IDP", idpConfigurations)))
        .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
        });

        services.AddHttpClient("UAT_DATA", client =>
         {
             client.BaseAddress = new Uri("https://its-idmuat-web.ad.unc.edu/services/data.api/v1/");
             client.Timeout = new TimeSpan(0, 1, 0);
         })
         .AddHttpMessageHandler(provider => new BearerTokenHandler(new TokenService("UAT_IDP", idpConfigurations)))
         .ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
         {
             ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
         });

    }





}