using Microsoft.AspNetCore.Http;
namespace signal_2019_dotnetcore.Extensions
{
    public static class SessionExtensions
    {
        public static void SetUsername(this ISession session, string username)
        {
            session.SetString("username", username);
        }

        public static string GetUsername(this ISession session)
        {
            return session.GetString("username");
        }
    }
}