namespace DatingApp.Api.Helper
{
    public class Hashing
    {
        
        public static (byte[] passwordHash, byte[] passwordSalt) CreatePasswordHash(string password)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                byte[] passwordSalt = hmac.Key;
                byte[] passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return (passwordHash, passwordSalt);
            }
        }
    }
}