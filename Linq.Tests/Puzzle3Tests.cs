using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Optional;
using Xunit;
using static System.Collections.IEnumerable;

namespace Linq.Tests
{
    public class Puzzle3Tests
    {
        [Fact]
        public void TestQ3_1ReturnsCorrectAnswer()
        {
            var result =
                Puzzle3Solver.Q3_1GetLongestConsecutiveNoSales(
                    "1,2,1,1,0,3,1,0,0,2,4,1,0,0,0,0,2,1,0,3,1,0,0,0,6,1,3,0,0,0");
            result.ValueOr(0).Should().Be(4);
        }

        [Fact]
        public void TestQ3_1HandlesNullInput()
        {
            var result = Puzzle3Solver.Q3_1GetLongestConsecutiveNoSales(null);
           result.Should().Be(10.None());
        }

        [Fact]
        public void TestQ3_1HandlesWhitespace()
        {
            var input = "1, 2,3 ,  4 ,   0  ,0, 0 ,3,0 ,2,1";
            var result = Puzzle3Solver.Q3_1GetLongestConsecutiveNoSales(input);
            result.ValueOr(0).Should().Be(3);
        }

        // NOTE: Xapien as Option extension method called .MaxOrNone()
        [Fact]
        public void TestQ3_1ReturnsZeroWhenAllSales()
        {
            //var list = new List<string> {"One", "Two", "Of", "Yo"};
            //var maxWords = list.MaxBy(x => x.Length);    // TODO: Neither I nor Rider can seem to find out how to import/use this? Is only in .Net 6. NuGet Import?
            var input = "1, 2, 3, 4, 5";
            var result = Puzzle3Solver.Q3_1GetLongestConsecutiveNoSales(input);
            result.ValueOr(1).Should().Be(0);
        }

        [Fact]
        public void TestQ3_2ReturnsCorrectAnswer()
        {
            var input = "4♣ 5♦ 6♦ 7♠ 10♥;10♣ Q♥ 10♠ Q♠ 10♦;6♣ 6♥ 6♠ A♠ 6♦;2♣ 3♥ 3♠ 2♠ 2♦;2♣ 3♣ 4♣ 5♠ 6♠";
            var expected = new List<string> {"10♣ Q♥ 10♠ Q♠ 10♦", "2♣ 3♥ 3♠ 2♠ 2♦"};
            var result = Puzzle3Solver.Q3_2WhereFullHouses(input);
            result.Should().Equal(expected);
        }

        [Fact]
        public void TestQ3_2HandlesNoFullHouses()
        {
            var input = "4♣ 5♦ 6♦ 7♠ 10♥;  2♣ 3♣ 4♣ 5♠ 6♠";
            var result = Puzzle3Solver.Q3_2WhereFullHouses(input);
            result.Should().BeEmpty();
        }

        [Fact]
        public void TestQ3_2IgnoresUnderOrOverdrawnHands()
        {
            // TODO: Does not check for correct length hands, but 4 cards cannot satisfy condition.
            // ♠♣♦♠
            var input = "4♠ 4♦;  2♠ 2♦ 2♣ 5♠;  2♣ 3♥ 3♠ 2♠ 2♦;  2♣ 2♠ 2♦ 3♠ 3♠ 3♦";
            var expected = "2♣ 3♥ 3♠ 2♠ 2♦";
            var result = Puzzle3Solver.Q3_2WhereFullHouses(input);
            result.Should().Equal(expected);
        }

        [Fact]
        public void TestQ3_3ReturnsCorrectWeekDays()
        {
            var result = Puzzle3Solver.Q3_3DayOfWeekOfNextTenChristmas(2018);
            result.Should().Equal(new List<string>{"Tuesday", "Wednesday", "Friday", "Saturday", "Sunday", "Monday", "Wednesday", "Thursday", "Friday", "Saturday"});
        }

        [Fact]
        public void TestQ3_4ReturnsOnlyAnagrams()
        {
            var input = "parts,traps,arts,rats,starts,tarts,rat,art,tar,tars,stars,stray";
            var anagram = "star";
            var result = Puzzle3Solver.Q3_4FilterAnagrams(input, anagram);
            result.Should().Equal(new List<string> {"arts", "rats", "tars"});
        }

        [Fact]
        public void TestQ3_5FiltersMatchingInitials()
        {
            var input = "Santi Cazorla, Per Mertesacker, Alan Smith, Thierry Henry, Alex Song, Paul Merson, Alexis Sánchez, Robert Pires, Dennis Bergkamp, Sol Campbell";
            var result = Puzzle3Solver.Q3_5GroupAndFilterMatchingInitials(input).ToList();
            result.Should().HaveCount(3);
            result.Select(g => g.Key).Should().Equal(new List<string> {"SC", "PM", "AS"});
        }
    }
}