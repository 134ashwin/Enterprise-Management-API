using DMello.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMello.Domain.Interfaces
{
    public interface IUserRepository
    {
        #region Login Part 
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByRefreshTokenAsync(string refreshToken); 
        Task AddAsync(User user);
        Task UpdateAsync(User user); 
        Task<bool> IsEmailDuplicateAsync(string email);
        #endregion

        #region ForgotPassword Things
        // Save password reset token in database
        Task SaveResetTokenAsync(int userId, string token, DateTime expiry);

        // Update password after user enters new password
        Task UpdatePasswordAsync(int userId, string newPasswordHash);

        #endregion
    }
}
