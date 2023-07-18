using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using LinkTreeAPI.Data;
using LinkTreeAPI.Model;
using LinkTreeAPI.Model.LoginRequest;
using Microsoft.EntityFrameworkCore;
using System.Web;

namespace LinkTreeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DataDbContext _context;
        private readonly IConfiguration _configuration;

        public UserController(DataDbContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
        }


        [HttpGet("Users")]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _context.Users.ToListAsync());
        }


        [HttpGet("UserName")]
        public async Task<IActionResult> GetUser(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);

            if (user == null)
            {
                return BadRequest("No User");
            }
            return Ok(user);
        }



        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterRequest request)
        {
            if (_context.Users.Any(u => u.Email == request.Email) || _context.Users.Any(u => u.UserName == request.UserName))
            {
                return BadRequest("User (Email or Username) already exists.");
            }

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var user = new User
            {
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                UserName = request.UserName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                VerificationToken = CreateRandomToken()
            };

            // Check if it's the first registered user
            if (!_context.Users.Any())
            {
                user.Role = "admin"; // Assign "admin" role
                user.VerifiedAt = DateTime.Now;
            }
            else
            {
                user.Role = "user"; // Assign "user" role
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            string returnUrl = "https://localhost:7070/";

            string verificationLink = Url.Action("Verify", "User", new { token = user.VerificationToken, returnUrl = returnUrl }, Request.Scheme);


            await SendVerificationEmail(user.Email, verificationLink);

            return Ok("User successfully created. Verification email sent.");
        }



        [HttpPost("loginWithEmail")]
        public async Task<IActionResult> LoginEmail(UserLoginEmailRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null)
            {
                return BadRequest("User not found.");
            }

            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Wrong password.");
            }

            if (user.VerifiedAt == null)
            {
                return BadRequest("User not verified.");
            }

            //string token = CreateToken(user);

            return Ok(user);
        }
        [HttpPost("loginWithUserName")]
        public async Task<IActionResult> LoginUserName(UserLoginUserNameRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == request.UserName);

            if (user == null)
            {
                return BadRequest("User not found.");
            }

            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Wrong password.");
            }

            if (user.VerifiedAt == null)
            {
                return BadRequest("User not verified.");
            }

            //string token = CreateToken(user);

            return Ok(user);
        }
        [HttpPost("loginWithPhoneNumber")]
        public async Task<IActionResult> LoginPhone(UserLoginPhoneRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == request.PhoneNumber);

            if (user == null)
            {
                return BadRequest("User not found.");
            }

            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            {
                return BadRequest("Wrong password.");
            }

            if (user.VerifiedAt == null)
            {
                return BadRequest("User not verified.");
            }

            //string token = CreateToken(user);

            return Ok(user);
        }

        [HttpGet("verify")]
        public async Task<IActionResult> Verify(string token, string returnUrl)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.VerificationToken == token);

            if (user == null)
            {
                return BadRequest("Invalid token.");
            }

            user.VerifiedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            // Use the Url.IsLocalUrl method to prevent open redirects
            if (!string.IsNullOrEmpty(returnUrl))
            {
                // Redirect to the returnUrl if it is a local URL
                return Redirect(returnUrl);
            }

            // If the returnUrl is not a local URL or is empty, redirect to a default page
            return Ok(user.VerifiedAt);
        }


        [HttpPost("Forgot-password")]
        public async Task<IActionResult> FotgotPassword(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                return BadRequest("User Not Found");
            }


            user.PasswordResetToken = CreateRandomToken();
            user.ResetTokenExpires = DateTime.Now.AddDays(1);

            await _context.SaveChangesAsync();

            return Ok($"You may reset your password now.");
        }


        [HttpPost("Reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.PasswordResetToken == request.Token);

            if (user == null || user.ResetTokenExpires < DateTime.Now)
            {
                return BadRequest("Invalid Token");
            }


            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            user.PasswordResetToken = null;
            user.ResetTokenExpires = null;

            await _context.SaveChangesAsync();

            return Ok($"Password Succesfully resets.");
        }

        private async Task SendVerificationEmail(string email, string confirmationLink)
        {
            string messageBody = $"Click the link below to verify your email:<br/><a href=\"{confirmationLink}\">Verify</a>";

            using (MailMessage message = new MailMessage("noreplynika@gmail.com", email))
            {
                message.Subject = "Email Verification";
                message.Body = messageBody;
                message.IsBodyHtml = true;

                using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtpClient.Credentials = new NetworkCredential("noreplynika@gmail.com", "cdqwvhmdwljietwq");
                    smtpClient.EnableSsl = true;

                    try
                    {
                        await smtpClient.SendMailAsync(message);
                    }
                    catch (Exception)
                    {
                        // Handle any exception that occurs during the email sending process
                        // You can log the error or perform other error handling actions
                    }
                }
            }
        }



        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
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
