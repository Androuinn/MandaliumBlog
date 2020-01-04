using System.Threading.Tasks;
using Mandalium.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Mandalium.API.Data
{
       public class AuthRepository : IAuthRepository<Writer>
    {
        private readonly DataContext _context;
        public AuthRepository(DataContext context)
        {
            this._context = context;

        }

        public async Task<Writer> Login(string username, string password)
        {
            var user = await _context.Writers.FirstOrDefaultAsync(x => x.Username == username);
            if (user == null) { return null; }

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt)) { return null; }
            return user;
        }

        public async Task<Writer> Register(Writer user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.Writers.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> UserExists(string username)
        {
            if (await _context.Writers.AnyAsync(x => x.Username == username)) { return true; }
            return false;
        }


        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) { return false; }
                }
            }
            return true;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }



    }
}