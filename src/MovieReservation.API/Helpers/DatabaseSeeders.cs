// MovieReservation.API/Data/DatabaseSeeder.cs
using Microsoft.EntityFrameworkCore;
using MovieReservation.Domain;
using MovieReservation.Services;

namespace MovieReservation.API.Data;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(MovieReservationContext context)
    {
        // Si ya hay salas cargadas, no hacemos nada (idempotencia)
        if (await context.TheaterRooms.AnyAsync())
            return;

        var rooms = new List<TheaterRoom>
        {
            new TheaterRoom(name: "Sala 1", roomType: "2D", capacity: 200),
            new TheaterRoom(name: "Sala VIP", roomType: "VIP", capacity: 200)
        };

        await context.TheaterRooms.AddRangeAsync(rooms);
        await context.SaveChangesAsync();

        var seats = new List<Seat>();

        foreach (var room in rooms)
        {
            // Filas de la A a la J (10 filas)
            for (char row = 'A'; row <= 'J'; row++)
            {
                // 20 asientos por fila
                for (int number = 1; number <= 20; number++)
                {
                    seats.Add(new Seat(
                        rowLetter: row.ToString(),
                        seatNumber: number,
                        seatType: "regular",
                        theaterRoomId: room.Id
                    ));
                }
            }
        }

        await context.Seats.AddRangeAsync(seats);
        await context.SaveChangesAsync();
    }
}