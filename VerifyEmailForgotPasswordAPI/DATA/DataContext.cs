﻿using Microsoft.EntityFrameworkCore;
using VerifyEmailForgotPasswordAPI.Models;

namespace VerifyEmailForgotPasswordAPI.DATA
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer("Server = (localdb)\\pipawserver; DataBase= UserVrFrDB; Trusted_Connection =True;");
        }
        public DbSet<User> Users => Set<User>();
    }
}
