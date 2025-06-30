namespace PersonalWebsite.Core.Interfaces;

public interface IMarkdownStorage
{
    Task ArchiveObjectAsync(string objectKey);
    Task<string> GeneratePresignedUploadUrlAsync(string objectKey, TimeSpan expiration);
    string GetPublicUrl(string objectKey);
}
