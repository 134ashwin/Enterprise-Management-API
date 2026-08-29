using DMello.Domain.Interfaces;
using DMello.Domain.Models;
using DMello.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMello.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Login Part 

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> IsEmailDuplicateAsync(string email)
        {
            return await _context.Users
                .AnyAsync(u => u.Email == email);
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        #endregion

        #region ForgotPassword Things

        public async Task SaveResetTokenAsync(int userId, string token, DateTime expiry)
        {
            //var user = await _context.Users.FindAsync(userId);
            //if (user != null)
            //{
            //    user.ResetToken = token;
            //    user.ResetTokenExpiry = expiry;
            //    await _context.SaveChangesAsync();
            //}
        }

        public async Task UpdatePasswordAsync(int userId, string newPasswordHash)
        {
            //var user = await _context.Users.FindAsync(userId);
            //if (user != null)
            //{
            //    user.PasswordHash = newPasswordHash;
            //    user.ResetToken = null;        // Clear used token
            //    user.ResetTokenExpiry = null;  // Clear expiry
            //    await _context.SaveChangesAsync();
            //}
        }

        #endregion


    }
}
