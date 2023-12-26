namespace EZMap.Configuration
{
    internal record AutoMapperTargetMemberConfiguration(bool Inject,
                                                        string? SourceMemberName,
                                                        Func<object>? DefaultValueFactory,
                                                        Func<object, object, object>? EnrichFunction,
                                                        Func<object, object, CancellationToken, Task<object>>? AsyncEnrichFunction);
}
