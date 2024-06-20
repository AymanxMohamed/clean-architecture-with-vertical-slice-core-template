// ReSharper disable CollectionNeverUpdated.Global

using Serilog.Sinks.Elasticsearch;

namespace Core.Presentation.Api.Common.Settings.Serilog;

public class SerilogSettings
{
    public const string SectionName = "Serilog";
    
    public List<WriteToSettings> WriteTo { get; init; } = [];

    public ElasticsearchSinkOptions GetElasticSearchSinkOptions(HostBuilderContext context)
    {
        return WriteTo.FirstOrDefault(x => x.Name.Equals(
            value: ArgsSettings.SerilogArgsSettingsName, StringComparison.OrdinalIgnoreCase))?.Args
                   .ToElasticSearchSinkOptions(context) 
                           ?? throw new NotSupportedException(
                               message: "You have to add elastic search settings in serilog configurations");
    }
}