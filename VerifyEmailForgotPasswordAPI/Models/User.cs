namespace VerifyEmailForgotPasswordAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } =string.Empty;
        public Byte[] PasswordHash { get; set; } = new byte[32];
        public Byte[] PasswordSalt { get; set; } = new byte[32];
        public string? VerificationToken { get; set; }  
        public DateTime? VerifiedAt { get; set; }    
        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpiresAt { get; set; }
    }
}
