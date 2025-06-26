namespace PersonalWebsite.Core.Interfaces;

public interface IImageStorage
{
    Task DeleteObjectAsync(string objectKey);
    Task<string> GeneratePresignedUploadUrlAsync(string objectKey, TimeSpan expiration);
    string GetPublicUrl(string objectKey);
}
