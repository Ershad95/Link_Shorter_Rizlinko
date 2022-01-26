using LinkShorter.Models;
using LinkShorter.Repository;
using ShorterLink_RestApi.Models;

namespace LinkShorter.Services.LinkService;

public class LinkService : ILinkService
{
    private readonly IRepo<Link> _repo;
    public LinkService(IRepo<Link> repo)
    {
        _repo = repo;
    }
    public bool CheckOrginalLink(string url)
    {
        return _repo.GetAllItems().Any(x => x.OrginalUrl == url.Trim());
    }

    public bool CheckShortLink(string url)
    {
        return _repo.GetAllItems().Any(x => x.ShortUrl == url.Trim());
    }

    public string GenerateShortUrl(CreateShortLinkModel createShortLinkModel)
    {
        int index = 0;
        bool isCustomLink = !string.IsNullOrEmpty(createShortLinkModel.ShortUrl);
        if (string.IsNullOrEmpty(createShortLinkModel.ShortUrl))
            createShortLinkModel.ShortUrl = Guid.NewGuid().ToString().Substring(index, new Random().Next(3, 6));
        var checkShortUrl = CheckShortLink(createShortLinkModel.ShortUrl);
        if (isCustomLink && checkShortUrl)
            return string.Empty;
        else
        {
            while (checkShortUrl)
            {
                createShortLinkModel.ShortUrl = Guid.NewGuid().ToString().Substring(++index, new Random().Next(3, 6));
                checkShortUrl = CheckShortLink(createShortLinkModel.ShortUrl);
            }
        }
        SaveShortLink(new Link() { ShortUrl = createShortLinkModel.ShortUrl, Password = createShortLinkModel.Pass, OrginalUrl = createShortLinkModel.OrginalUrl, Active = true, UseCustomLink = string.IsNullOrEmpty(createShortLinkModel.ShortUrl) });
        return createShortLinkModel.ShortUrl;
    }

    public Link GetLinkById(int id)
    {
        return _repo.GetItemById(id);
    }

    public Link GetLinkByShortLink(string url)
    {
        var item = _repo.GetAllItems().FirstOrDefault(x => x.ShortUrl == url.Trim());
        if (item == null) return null;
        return item;
    }

    public string GetOrginalLinkByShortLink(string url)
    {
        var item = _repo.GetAllItems().FirstOrDefault(x => x.ShortUrl == url.Trim());
        if (item == null) return "";
        return item.OrginalUrl;
    }

    public bool SaveShortLink(Link link)
    {
        link.CreatedDateTime = DateTime.UtcNow;
        link.Active = true;
        _repo.InsertItem(link);
        return true;
    }
}
