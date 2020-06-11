using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Chroomsoft.Top2000.Data.Tests
{
    public class Position
    {
        public int TrackID { get; set; }

        public int Year { get; set; }

        [Column("Position")]
        public int TrackPosition { get; set; }
    }

    public class PlayTime
    {
        [Key]
        public int TrackID { get; set; }

        public int Year { get; set; }

        public DateTimeOffset DateAndTime { get; set; }
    }

    public class Top2000DbContext : DbContext
    {
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        public DbSet<PlayTime> PlayTime { get; set; }

        public DbSet<Position> Position { get; set; }

        public DbSet<PlayTimeAndPositionJoined> PlayTimeAndPositionJoined { get; set; }

#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=(localdb)\mssqllocaldb;Database=Top2000;Integrated Security=True");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlayTimeAndPositionJoined>().HasNoKey();
            modelBuilder.Entity<Position>().HasKey("TrackID", "Year");
            modelBuilder.Entity<PlayTime>().HasKey("TrackID", "Year");
        }
    }

    public class PlayTimeAndPositionJoined
    {
        public int TrackID { get; set; }

        public int Year { get; set; }

        public int TrackPosition { get; set; }

        public DateTimeOffset DateAndTime { get; set; }
    }

    [TestClass]
    public class DataTests
    {
        [TestMethod]
        public async Task EveryYearHas2000Positions()
        {
            using var db = new Top2000DbContext();

            var positions = await db.Position.ToListAsync();

            var grouped = positions.GroupBy(x => x.Year);

            foreach (var year in grouped)
            {
                var count = year.Count();
                if (count != 2000 && count != 10)
                {
                    Assert.Fail($"Year {year.Key} has { count} positions where 2000 or 10 where expected");
                }
            }
        }

        [TestMethod]
        public async Task EachPlayTimeYearHas2000Positions()
        {
            using var db = new Top2000DbContext();

            var positions = await db.PlayTime.ToListAsync();

            var grouped = positions.GroupBy(x => x.Year);

            foreach (var year in grouped)
            {
                var count = year.Count();
                if (count != 2000 && count != 10)
                {
                    Assert.Fail($"Year {year.Key} has { count} positions where 2000 or 10 where expected");
                }
            }
        }

        [TestMethod]
        public async Task ForEachYearTheTimeBetweenTwoPositionsIsIncreatedByOneHourOrSame()
        {
            using var db = new Top2000DbContext();
            var resultSet = await db.PlayTimeAndPositionJoined
            .FromSqlRaw(
            "SELECT p.TrackID AS TrackID, p.Year, p.DateAndTime, pos.Position AS TrackPosition FROM PlayTime p JOIN Position pos ON p.TrackID = pos.TrackID And p.Year = pos.Year"
            )
            .ToListAsync();

            var grouped = resultSet.GroupBy(x => x.Year);
            foreach (var year in grouped)
            {
                var tracks = year.OrderBy(x => x.TrackPosition).ToArray();

                var lastTrack = tracks.First();

                for (int i = 0; i < tracks.Length; i++)
                {
                    var sut = tracks[i];
                    var differenceSinceLast = lastTrack.DateAndTime - sut.DateAndTime;

                    Assert.IsTrue(differenceSinceLast.TotalMinutes == 0 || differenceSinceLast.TotalMinutes == 60,
                        $"The time difference between position {sut.TrackPosition} and {lastTrack.TrackPosition} is {differenceSinceLast.TotalMinutes} where only 0 or 60 is allowed. Year = {sut.Year}, TrackId = {sut.TrackID}");

                    lastTrack = sut;
                }
            }
        }
    }
}
