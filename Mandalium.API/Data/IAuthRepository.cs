using System.Threading.Tasks;

namespace Mandalium.API.Data
{
    public interface IAuthRepository<T>
    {
          Task<T> Register(T user, string password);
         Task<T> Login(string username, string password);
         Task<bool> UserExists(string username);
    }
}