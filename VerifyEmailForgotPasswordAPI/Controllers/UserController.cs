using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using VerifyEmailForgotPasswordAPI.DATA;
using VerifyEmailForgotPasswordAPI.Models;

namespace VerifyEmailForgotPasswordAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly DataContext _context;
        public UserController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterRequest request)
        {
            if (_context.Users.Any(u => u.Email == request.Email))
            {
                return BadRequest("User already exists.");
            }

            CreatePasswordHash(request.Password,
                out Byte[] PasswordHash,
                out Byte[] PasswordSalt);

            var user = new User
            {
                Email = request.Email,
                PasswordHash = PasswordHash,
                PasswordSalt = PasswordSalt,
                VerificationToken = CreateRandomToken()
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User Successfully Registered. Please verify your email.");
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
            {
                return BadRequest("Invalid Email or Password");
            }

            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Invalid Email or Password ;)");
            }

            if (user.VerifiedAt == null)
            {
                return BadRequest("Please verify your email first.");
            }


            return Ok($"Login Successful, Welcome Back {request.Email} :)");
        }

        [HttpPost("Verify")]
        public async Task<IActionResult> Verify(string token)
        {
             var user = await _context.Users.FirstOrDefaultAsync(u => u.VerificationToken == token);
            if (user == null)
            {
                return BadRequest("Invalid token .");
            }
            user.VerifiedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return Ok("User Verified! :)");
        }
        
        [HttpPost("forgot_password")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
             var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return BadRequest("User Not Found .");
            }
            user.PasswordResetToken = CreateRandomToken();
            user.PasswordResetTokenExpiresAt = DateTime.Now.AddDays(1);
            await _context.SaveChangesAsync();

            return Ok("You may now reset your password :)");
        }
        
        [HttpPost("reset_password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
             var user = await _context.Users.FirstOrDefaultAsync(u => u.PasswordResetToken == request.Token);
            if (user == null || user.PasswordResetTokenExpiresAt < DateTime.Now )
            {
                return BadRequest("Invalid token .");
            }
            
            CreatePasswordHash(request.Password,
                out Byte[] PasswordHash,
                out Byte[] PasswordSalt);

            user.PasswordHash = PasswordHash;
            user.PasswordSalt = PasswordSalt;
            user.PasswordResetToken = null;
            user.PasswordResetTokenExpiresAt = null;

            await _context.SaveChangesAsync();

            return Ok("You may now reset your password :)");
        }



        private void CreatePasswordHash(string password, out Byte[] passwordHash, out Byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
        private string CreateRandomToken()
        {
            return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
        }

    }
}
