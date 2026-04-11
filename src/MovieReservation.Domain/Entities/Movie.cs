namespace MovieReservation.Domain;

public class Movie
{
    public const int DescriptionMaxLength = 400;
    public const int NameMaxLength = 100;
    
    public long Id { get; set; }

    public string Name { get; private set; } = default!;

    public string? Description { get; private set; } = default!;

    public DateTime ShowTime { get; set; }

    public Movie(string name, string? description = null)
    {
        SetName(name);
        SetDescription(description);
    }


    public void SetName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty.", nameof(name));

        if (name.Length > NameMaxLength)
            throw new ArgumentException($"Name cannot exceed {NameMaxLength} characters.", nameof(name));

        Name = name.Trim();
    }

    public void SetDescription(string? description)
    {
        if (description is not null && description.Length > DescriptionMaxLength)
            throw new ArgumentException($"Description cannot exceed {DescriptionMaxLength} characters.", nameof(description));

        Description = description?.Trim();
    }
    
}
