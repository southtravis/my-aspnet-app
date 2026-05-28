using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Dapper;

namespace MyAspNetApp.Api.Controllers;

public class AccountController : Controller
{
    private readonly IConfiguration _config;
    public AccountController(IConfiguration config)
    {
        _config = config;
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string username, string password)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            ViewBag.Error = "Username and password are required.";
            return View();
        }

        var connStr = _config.GetConnectionString("DefaultConnection");
        using var conn = new SqlConnection(connStr);
        var user = await conn.QuerySingleOrDefaultAsync<User>(
            "SELECT Id, Username, PasswordHash, Salt FROM [dbo].[Users] WHERE Username = @Username",
            new { Username = username });

        if (user == null)
        {
            ViewBag.Error = "Invalid username or password.";
            return View();
        }

        // Verify password (simple salted hash using SHA256 for demo; in production use a stronger KDF)
        var hash = ComputeHash(password, user.Salt);
        if (!string.Equals(hash, user.PasswordHash, StringComparison.Ordinal))
        {
            ViewBag.Error = "Invalid username or password.";
            return View();
        }

        // For demo, just show a welcome page
        return View("Welcome", user);
    }

    private static string ComputeHash(string password, string salt)
    {
        using var sha = System.Security.Cryptography.SHA256.Create();
        var bytes = System.Text.Encoding.UTF8.GetBytes(salt + password);
        var hash = sha.ComputeHash(bytes);
        return Convert.ToHexString(hash);
    }

    private class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Salt { get; set; } = string.Empty;
    }
}
