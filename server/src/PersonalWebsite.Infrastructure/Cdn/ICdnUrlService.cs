namespace PersonalWebsite.Infrastructure.Cdn;

public interface ICdnUrlService
{
    public string ConvertToCdnUrl(string rawUrl, string storageProvider);
}
