namespace ClearMeasure.Client;

internal static class Program
{
    private static void Main(string[] args)
    {
        var host = HostBuilder(args)
            .UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureContainer<ContainerBuilder>(ConfigureContainer)
            .ConfigureServices(ConfigureServices)
        .Build();

        host.Run();

    }

    private static IHostBuilder HostBuilder(string[] args)
    {
        // Create the host builder to start the app
        var builder = Host.CreateDefaultBuilder(args);

        // Setup application logging settings
        builder.ConfigureLogging((hostBuilderContext, loggingBuilder) =>
        {
            var loggingLevel = hostBuilderContext.HostingEnvironment.IsDevelopment() ?
                LogLevel.Information : LogLevel.Warning;

            loggingBuilder.SetMinimumLevel(loggingLevel);

            loggingBuilder.AddFilter("Microsoft", LogLevel.Information);

            if (!hostBuilderContext.HostingEnvironment.IsProduction())
            {
                loggingBuilder.AddJsonConsole(options =>
                {
                    options.JsonWriterOptions = new JsonWriterOptions {Indented = true};
                });
                loggingBuilder.AddDebug();
            }
        });

        return builder;
    }

    private static void ConfigureContainer(HostBuilderContext hostContext, ContainerBuilder builder)
    {
        var fileString = System.Text.Encoding.Default.GetString(Resource.valueName);
        var deserializedTuple = JsonConvert.DeserializeObject<Tuple<int, string>[]>(fileString);
        
        builder.RegisterModule(new LibraryModule(deserializedTuple));
    }

    private static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
    {
        services.AddHostedService<Service>();
    }
}
