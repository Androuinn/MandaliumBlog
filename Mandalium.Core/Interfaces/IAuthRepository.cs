using System.Threading.Tasks;

namespace Mandalium.Core.Interfaces
{
    public interface IAuthRepository<T>
    {
          Task<T> Register(T user, string password);
         Task<T> Login(string username, string password);
         Task<bool> UserExists(string username);
    }
}