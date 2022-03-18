using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MusicEvents.DataAccess;
using MusicEvents.Entities;

namespace MusicEvents.UnitTest;

public class DbContextUnitTest : IDisposable
{
    protected readonly MusicalEventsDbContext Context;

    protected DbContextUnitTest()
    {
        var options = new DbContextOptionsBuilder<MusicalEventsDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        Context = new MusicalEventsDbContext(options);

        Context.Database.EnsureCreated();

        Seed(Context);
    }

    private void Seed(MusicalEventsDbContext context)
    {
        var genre = new Genre
        {
            Description = "Rock",
            Status = true
        };
        Context.Set<Genre>().Add(genre);

        var list = new List<Concert>();
        var random = new Random();

        for (int i = 0; i < 100; i++)
        {
            var valor = random.Next(20, 400);

            list.Add(new Concert
            {
                Description = $"Event random at {valor}",
                Title = $"Event {i}",
                Genre = genre,
                Status = true,
                TicketsQuantity = random.Next(),
                UnitPrice = Convert.ToDecimal(random.Next(500, 6400)),
                DateEvent = new DateTime(random.Next(2018, 2022), random.Next(1,12), random.Next(1, 28)),
                Finalized = false
            });
        }

        Context.Set<Concert>().AddRange(list);
        Context.SaveChanges();
    }

    public void Dispose()
    {
        Context?.Dispose();
    }
}