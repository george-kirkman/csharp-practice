using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using FluentAssertions;
using Optional.Collections;

namespace Linq.Tests
{
    public class Puzzle1Tests
    {
        // NOTE: We can use [Theory] and [InlineData] to repeat a test many times with diff params!
        // We should use this sparingly - part of the value of tests is specific method names for when things go wrong.
        // https://andrewlock.net/creating-parameterised-tests-in-xunit-with-inlinedata-classdata-and-memberdata/
        [Theory]
        [InlineData("Davis, Clyne, Fonte, Hooiveld", "1. Davis, 2. Clyne, 3. Fonte, 4. Hooiveld")]
        [InlineData("George, Joseph, Sam", "1. George, 2. Joseph, 3. Sam")]
        public void TestQ1_1GivesCorrectAnswerFromExample(string names, string expected)
        {
            var result = Puzzle1Solver.Q1_1GivePlayersShirtNumbers(names);
            result.Should().Be(expected);
        }

        [Fact]
        public void TestQ1_1HandlesDifferentWhiteSpace()
        {
            const string input = " George , Joseph,Sam";
            const string expected = "1. George, 2. Joseph, 3. Sam";
            var result = Puzzle1Solver.Q1_1GivePlayersShirtNumbers(input);
            result.Should().Be(expected);
        }

        [Fact]
        public void TestQ1_1HandlesFullNames()
        {
            const string input = "George Kirkman, Joseph Whiting";
            const string expected = "1. George Kirkman, 2. Joseph Whiting";
            var result = Puzzle1Solver.Q1_1GivePlayersShirtNumbers(input);
            result.Should().Be(expected);
        }
        
        [Fact]
        public void TestQ1_2CorrectAnswerFromExample()
        {
            var playersRaw = "Jason Puncheon, 26/06/1986; Jos Hooiveld, 22/04/1983; Kelvin Davis, 29/09/1976; Luke Shaw, 12/07/1995; Gaston Ramirez, 02/12/1990; Adam Lallana, 10/05/1988";
            var result = Puzzle1Solver.Q1_2SortPlayersByAge(playersRaw).ToList();
            result.First().Name.Should().Be("Kelvin Davis");
            result.First().Age.Should().Be(45);
        }

        [Fact]
        public void TestQ1_2HandlesDoBFromTheFuture()
        {
            var playersRaw = "George Kirkman, 27/04/1998; Johnny Silverhand, 01/01/2077";
            var result = Puzzle1Solver.Q1_2SortPlayersByAge(playersRaw).ToList();
            result.First().Name.Should().Be("George Kirkman");
            result.Last().Age.Should().Be(-56);
        }

        [Fact]
        public void TestQ1_2CorrectAgeWithLeapYearsInbetween()
        {
            var playerWithLeapYear = "George Kirkman, 20/11/2019";
            var result = Puzzle1Solver.Q1_2SortPlayersByAge(playerWithLeapYear).ToList();
            result.First().Age.Should().Be(1);
        }

        [Fact]
        public void TestQ1_2HandlesWronglyOrderedDoBAndName()
        {
            var badInput = "George Kirkman, 27/04/1998; 01/01/0001, Jesus Christ";
            var result = Puzzle1Solver.Q1_2SortPlayersByAge(badInput).ToList();
            result.First().Name.Should().Be("Jesus Christ");
            result.First().Age.Should().Be(2020); // TODO: Edit this (and other ages) to handle running this test in the future.
        }
        
        //TODO
        [Fact]
        public void TestQ1_2IgnoresEntryWithMissingDoB()
        {
            var badInput = "George Kirkman, 27/04/1998; Donald Trump; Elon Musk, 28/06/1971";
            var result = Puzzle1Solver.Q1_2SortPlayersByAge(badInput).ToList();
            result.Should().HaveCount(2);
            result.First().Name.Should().Be("Elon Musk");
            result.Last().Name.Should().Be("George Kirkman");
        }
        
        [Fact]
        public void TestQ1_3CorrectAnswerFromExample()
        {
            var albumTimes = "4:12,2:43,3:51,4:29,3:24,3:14,4:46,3:25,4:52,3:27";
            var result = Puzzle1Solver.Q1_3CalculateTotalAlbumLength(albumTimes);
            result.Should().Be(new TimeSpan(0, 38, 23));
        }

        [Fact]
        public void TestQ1_4CorrectAnswerFromExample()
        {
            var result = Puzzle1Solver.Q1_4GenerateGridCoords(3, 3);
            var expected = new List<string> {"0, 0", "0, 1", "0, 2", "1, 0", "1, 1", "1, 2", "2, 0", "2, 1", "2, 2"};
            result.Should().Equal(expected);
        }

        [Fact]
        public void TestQ1_5CorrectAnswerFromExample()
        {
            var result = Puzzle1Solver.Q1_6ExpandRanges("2,5,7-10,11,17-18");
            var expected = new List<int> {2, 5, 7, 8, 9, 10, 11, 17, 18};
            result.Should().Equal(expected);
        }
    }
}