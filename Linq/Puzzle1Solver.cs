using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;

namespace Linq
{
    public static class Puzzle1Solver
    {

        /// <summary>
        /// FOR QUESTION #1.1: Adds numbers to string of names
        /// e.g."Davis, Clyne, Fonte, ..." --> "1. Davis, 2. Clyne, 3. Fonte"
        /// </summary>
        /// <param name="names">String of comma separated names</param>
        /// <returns>String of comma separated *and numbered* names</returns>
        public static string Q1_1GivePlayersShirtNumbers(string names)
        {
            return string.Join(", ", names
                .Split(",")
                .Select((name, i) => $"{i+1}. " + name.Trim()));
        }


        // TODO: Alternative method that doesn't try to fix bad inputs, but instead skips them + only operates on good inputs/returns None.
        public static IEnumerable<Player> Q1_2SortPlayersByAge(string players)
        {
            return players
                .Split(";")
                .Select(p =>
                    {
                        var nameAndDate = p.Trim().Split(",").Select(n => n.Trim());
                        var date = nameAndDate.First(x => DateTime.TryParse(x,new CultureInfo("en-GB"), DateTimeStyles.None, out DateTime DoB));
                        var name = nameAndDate.Except<string>(new List<string>{date}).First();
                        return new Player(name, date);
                    }
                ).OrderByDescending(p => p.Age);
        }

        public static TimeSpan Q1_3CalculateTotalAlbumLength(string albumTimes)
        {
            return albumTimes
                .Split(',')
                .Aggregate(TimeSpan.Zero, (acc, next) => acc + TimeSpan.Parse($"0:{next}"));
        }

        public static IEnumerable<string> Q1_4GenerateGridCoords(int x, int y)
        {
            return Enumerable.Range(0, x)
                .SelectMany(x => Enumerable.Range(0, y)
                    .Select(y => $"{x}, {y}"));
        }

        //TODO: Q1_5 goes here
        //
        //
        //
        
        
        public static IEnumerable<int> Q1_6ExpandRanges(string ranges)
        {
            var temp = "2,5,7-10,11,17-18";
            return temp
                .Split(',')
                .SelectMany(x =>
                    {
                        var bounds = x.Split('-').Select(int.Parse).ToList();
                        return Enumerable.Range(bounds.First(), bounds.Last() - bounds.First() + 1);
                    }
                );
        }
    }

    public class Player
    {
        public string Name { get; set; }
        public DateTime DoB { get; set; }

        public int Age
        {
            get
            {
                var age = DateTime.Today.Year - DoB.Year;
                if (DoB.Date > DateTime.Today.AddYears(-age)) age--;
                return age;
            }
        }

        public Player(string name, string doB)
        {
            Name = name;
            DoB = DateTime.Parse(doB, new CultureInfo("en-GB"));
            Console.WriteLine(DoB);
        }
    }
}