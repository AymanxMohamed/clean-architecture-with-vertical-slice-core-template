using Serilog.Sinks.Elasticsearch;

namespace ProjectName.Presentation.Api.Common.Settings.Serilog;

public sealed class ArgsSettings
{
    public const string SerilogArgsSettingsName = "Elasticsearch";
    
    public string NodeUris { get; init; } = null!;
    
    public bool AutoRegisterTemplate { get; init; }
    
    public AutoRegisterTemplateVersion? AutoRegisterTemplateVersion { get; init; }
    
    public int NumberOfShards { get; init; }
    
    public int NumberOfReplicas { get; init; }
    
    public ElasticsearchSinkOptions ToElasticSearchSinkOptions(HostBuilderContext context)
    {
        return new ElasticsearchSinkOptions(node: new Uri(NodeUris))
        {
            AutoRegisterTemplate = AutoRegisterTemplate,
            IndexFormat = ($"{context.Configuration["ApplicationName"]}-logs-" +
                           $"{context.HostingEnvironment.EnvironmentName}" +
                           $"-{DateTime.UtcNow:yyyy-MM}").ToLower().Replace('.', '-'),
            AutoRegisterTemplateVersion = AutoRegisterTemplateVersion,
            NumberOfReplicas = NumberOfReplicas,
            NumberOfShards = NumberOfShards,
        };
    }
}