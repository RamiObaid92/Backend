using Microsoft.Extensions.Configuration;

namespace Business.Services;

public interface IImageCatalogService
{
    IReadOnlyList<string> GetAvatars();
    string GetDefaultAvatar();
    IReadOnlyList<string> GetProjectPics();
}

public class ImageCatalogService(IConfiguration configuration) : IImageCatalogService
{
    private readonly List<string> _avatars = configuration.GetSection("ImageCatalog:AvatarUrls").Get<List<string>>() ?? [];
    private readonly List<string> _projects = configuration.GetSection("ImageCatalog:ProjectUrls").Get<List<string>>() ?? [];

    public IReadOnlyList<string> GetAvatars() => _avatars;
    public IReadOnlyList<string> GetProjectPics() => _projects;
    public string GetDefaultAvatar()
        => _avatars.FirstOrDefault() ?? "avatar1.png";
}
