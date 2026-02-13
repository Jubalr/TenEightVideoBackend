namespace TenEightVideo.Web.Updates
{
    public interface IUpdateChecker
    {
        string GetLatestVersion(string appKey);
    }
}
