using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using MoreLinq;
using Optional;
using Optional.Collections;

namespace Linq
{
    public static class Puzzle3Solver
    {
        // Xapien MaxOrNone() extension method could change this. Or MaxBy() returns a list 
        public static Option<int> Q3_1GetLongestConsecutiveNoSales(string input)
        {
            // New method uses MoreLinq MaxBy()
            var alternative = input.SomeNotNull()
                .FlatMap(s => s.Split(',')
                    .Select(x => x.Trim() == "0" ? "N" : "Y")
                    .Aggregate("", (acc, next) => acc + next)
                    .Split(new[] {"Y"}, StringSplitOptions.RemoveEmptyEntries)
                    .MaxBy(x => x.Length).ToList()
                    .FirstOrNone().Map(x => x.Length));
            
            // NOTE: OLD METHOD using DefaultIfEmpty
            var result = input.SomeNotNull()
                .Map(s => s.Split(',')
                    .Select(x => x.Trim() == "0" ? "N" : "Y")
                    .Aggregate("", (acc, next) => acc + next)
                    .Split(new[] {"Y"}, StringSplitOptions.RemoveEmptyEntries)
                    .DefaultIfEmpty("")
                    .Max(x => x.Length)
                );
            return alternative;
        }

        public static IEnumerable<string> Q3_2WhereFullHouses(string input)
        {
            return input
                .Split(';')
                .Where(hand =>
                {
                    var cards = hand
                        .Trim()
                        .Split(' ').ToImmutableList();
                    return cards.Count == 5 && cards
                        .GroupBy(card => card.First()).ToImmutableList()
                        .All(group => group.Count() == 2 || group.Count() == 3);
                })
                .Select(hand => hand.Trim());
        }

        public static IEnumerable<string> Q3_3DayOfWeekOfNextTenChristmas(int year)
        {
            return Enumerable.Range(year, 10)
                .Select(year => new DateTime(year, 12, 25).DayOfWeek.ToString());
        }

        public static IEnumerable<string> Q3_4FilterAnagrams(string words, string anagram)
        {
            string Sort(string s) => new string(s.OrderBy(c => c).ToArray());
            return words
                .Split(',')
                .Where(word => Sort(word) == Sort(anagram));
        }

        public static IEnumerable<IGrouping<string, string>> Q3_5GroupAndFilterMatchingInitials(string names)
        {
            return names.Split(", ")
                .GroupBy(fullName =>
                {
                    var names = fullName.Split(' ');
                    return "" + names.First().First() + names.Last().First();
                }).Where(g => g.Count() > 1);
        }

        public static string Q3_6ReturnChoppedTimes(string timesToChop, string originalDuration)
        {
            var temp = "0:00:00-0:00:05;0:55:12-1:05:02;1:37:47-1:37:51";
            //TODO: Investigate MoreLINQ and how to use it here instead of "ugly" pure LINQ.
            return "placeholder";
        }
    }
}