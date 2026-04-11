namespace MovieReservation.Services;

using BCrypt.Net;

public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hash);
}

public class BcryptPasswordHasher : IPasswordHasher
{
    public string HashPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("La contraseña no puede estar vacía", nameof(password));

        // BCrypt.HashPassword ya incluye salt automáticamente
        return BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string password, string hash)
    {
        if (string.IsNullOrWhiteSpace(password))
            return false;

        if (string.IsNullOrWhiteSpace(hash))
            return false;

        try
        {
            return BCrypt.Verify(password, hash);
        }
        catch
        {
            return false;
        }
    }
}