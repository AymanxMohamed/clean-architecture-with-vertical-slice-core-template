namespace ProjectName.Presentation.Api.Common.Settings.Serilog;

public sealed class WriteToSettings
{
    public string Name { get; init; } = null!;
    
    public ArgsSettings Args { get; init; }
}