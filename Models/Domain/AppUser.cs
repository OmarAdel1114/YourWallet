namespace YourWallet.Models.Domain;

public class AppUser
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
    public string? ProfilePictureUrl { get; set; }
    public string GoogleId { get; set; } = string.Empty; // unique ID from Google
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<Expense> Expenses { get; set; } = new List<Expense>();
}