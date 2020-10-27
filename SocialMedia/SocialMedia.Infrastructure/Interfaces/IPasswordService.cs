namespace SocialMedia.Infrastructure.Interfaces
{
    public interface IPasswordService
    {
        string Hash(string password);
        bool check(string hash,string password);
    }
}
