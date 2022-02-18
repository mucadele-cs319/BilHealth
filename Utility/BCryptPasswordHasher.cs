using Microsoft.AspNetCore.Identity;
using BilHealth.Model;

namespace BilHealth.Utility
{
    using BCrypt.Net;

    public class BCryptPasswordHasher : IPasswordHasher<User>
    {
        public string HashPassword(User user, string password)
        {
            return BCrypt.HashPassword(password, 12);
        }

        public PasswordVerificationResult VerifyHashedPassword(User user, string hashedPassword, string givenPassword)
        {
            return BCrypt.Verify(givenPassword, hashedPassword) ?
                PasswordVerificationResult.Success :
                PasswordVerificationResult.Failed;
        }
    }
}
