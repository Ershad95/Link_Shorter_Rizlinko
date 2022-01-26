using LinkShorter.Models;
using ShorterLink_RestApi.Models;

namespace LinkShorter.Services.LinkService;

public interface ILinkService
{
    public bool CheckShortLink(string url);
    public bool CheckOrginalLink(string url);
    public bool SaveShortLink(Link link);

    public string GetOrginalLinkByShortLink(string url);
    public Link GetLinkByShortLink(string url);
    public Link GetLinkById(int id);

    public string GenerateShortUrl(CreateShortLinkModel createShortLinkModel);
}
