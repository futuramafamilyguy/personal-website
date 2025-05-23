﻿namespace PersonalWebsite.Infrastructure.MarkdownStorage;

public class MarkdownStorageConfiguration
{
    public required string Provider { get; set; }
    public required string Host { get; set; }
    public required string BasePath { get; set; }
    public required bool CdnEnabled { get; set; }
}
