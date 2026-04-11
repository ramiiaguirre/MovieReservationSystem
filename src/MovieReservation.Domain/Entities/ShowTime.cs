using System;

namespace MovieReservation.Domain.Entities;

public class ShowTime
{
    public long Id { get; set; }

    public DateTime ShowDateTime { get; set; }

    public decimal Price { get; set; }

    public bool IsActive { get; set; }
}
