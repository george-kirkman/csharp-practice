using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Optional.Collections;

namespace Linq
{
    class Program
    {
        static void Main(string[] args)
        {
            //QUESTION 1
            string names =
                "Davis, Clyne, Fonte, Hooiveld, Shaw, Davis, Schneiderlin, Cork, Lallana, Rodriguez, Lambert";
            
            // ATTEMPT
            string output = String.Join('.', Enumerable.Zip(Enumerable.Range(1, names.Length+1), names.Split(',')));
            
            // ANSWER
            output = String.Join(',', names
                .Split(',')
                .Select((item, index) => index + 1 + "." + item)
                .ToArray());
            //Console.WriteLine(output);
            
            //QUESTION 2
            var input = "Jason Puncheon, 26/06/1986; Jos Hooiveld, 22/04/1983; Kelvin Davis, 29/09/1976; Luke Shaw, 12/07/1995; Gaston Ramirez, 02/12/1990; Adam Lallana, 10/05/1988";
            var orderedAgedPeople = input
                .Split(';')
                .OrderBy(nameAgeString => DateTime.ParseExact(nameAgeString.Split(',').Last().Trim(), "dd/MM/yyyy",
                    CultureInfo.InvariantCulture)
                );

            
            // PLURALSIGHT COURSE
            // motorcycle challenge - sum of scores excluding 3 worst.
            string scores = "10,5,0,8,10,1,4,0,10,1";
            var scoresSorted = scores
                .Split(',')
                .Select(int.Parse)  // Cast elements to int before ordering!
                .OrderBy(x => x);
            //Console.WriteLine(String.Join(",", scoresSorted));
            var totalScore = scoresSorted
                .Skip(3)
                .Sum();
            //Console.WriteLine(totalScore);

            // Album length challenge
            var albumTimes = "2:54, 3:48, 4:51,3:32,6:15,4:08,5:17,3:13,4:16,3:55,4:53,5:35,4:24";
            var totalTime = albumTimes
                .Split(',')
                .Select(x => "0:" + x.Trim())
                .Select(TimeSpan.Parse)
                .Aggregate((t1, t2) => t1 + t2);
            Console.WriteLine(totalTime);
            
            // Expand array with ranges
            var ranges = "2,5,7-10,11,17-18";
            var ranges2 = "6,1-3,2-4";
            var expandedRange = ranges2
                .Split(',')
                .Select(x => x.Split('-'))
                .Select(p => new {First = int.Parse(p.First()), Last = int.Parse(p.Last())})   //Anonymous class
                .SelectMany(fl => Enumerable.Range(fl.First, fl.Last - fl.First + 1))
                .Distinct().OrderBy(x => x);
            
            // Create an enumerable sequence of strings in the form "x,y" representing all the points on a 3x3 grid. e.g. output would be: 0,0 0,1 0,2 1,0 1,1 1,2 2,0 2,1 2,2
            var coords = Enumerable.Range(0, 3).
                Select(x => Enumerable.Range(0, 3)
                    .Select(y => (x, y)))
                .SelectMany(coords => coords
                    .Select(coord => string.Join(",", coord.x, coord.y)));
            
            // ^^^ Mark's Simpler Answer:
            coords = Enumerable.Range(0, 3)
                .SelectMany(x => Enumerable.Range(0, 3)
                    .Select(y => $"{x},{y}"));
            
            //  "00:45,01:32,02:18,03:01,03:44,04:31,05:19,06:01,06:47,07:35" which represents the times (in minutes and seconds) at which a swimmer completed each of 10 lengths. 
            // Turn into IEnumerable of TimeSpan objects with time taken for each length.
            var timesRaw = "00:45,01:32,02:18,03:01,03:44,04:31,05:19,06:01,06:47,07:35";
            var times = timesRaw
                .Split(',')
                .Select(s => TimeSpan.Parse($"00:{s}"));
            var timeDiffs = times
                .Select((t, index) => t - times.Skip(index - 1).First());
            
            
            // MARK HEATH LINQ PUZZLES #3 (THE HARDER ONES!)
            
            /////// QUESTION ONE: return longest sequence of days without a sale //////////////
            // Needed a hint for this one.
            var salesPerDay = "1,2,1,1,0,3,1,0,0,2,4,1,0,0,0,0,2,1,0,3,1,0,0,0,6,1,3,0,0,0";
            var longestNoSales = salesPerDay
                .Split(',')
                .Select(x => x == "0" ? "N" : "Y")
                .Aggregate("", (agg, next) => agg + next)
                .Split(new[] {"Y"}, StringSplitOptions.RemoveEmptyEntries)
                .Max(n => n.Length);
            
            var longestNoSales1 = salesPerDay
                .Split(',')
                .Select(x => x == "0" ? "N" : "Y")
                .Aggregate("", (agg, next) => agg + next);
            var longestNoSales2 = longestNoSales1.Split(new[] {"Y"}, StringSplitOptions.RemoveEmptyEntries);
            var longestNoSales3 = longestNoSales2.Max(n => n.Length);
            Console.WriteLine(longestNoSales);
            
            //////////// QUESTION TWO: return a sequence containing "Full House" hands //////////////
            var pokerHands = "4♣ 5♦ 6♦ 7♠ 10♥;10♣ Q♥ 10♠ Q♠ 10♦;6♣ 6♥ 6♠ A♠ 6♦;2♣ 3♥ 3♠ 2♠ 2♦;2♣ 3♣ 4♣ 5♠ 6♠";
            var separateHands = pokerHands
                .Split(';')
                .Select(handString => handString.Split(' ').Select(cardStr =>  cardStr.First() )); // Handle bad data?
            var handsGroupings = separateHands
                .Select(hand => hand.GroupBy(x => x));
            var fullHouses = handsGroupings
                .Where(handGroupings =>
                    handGroupings.Count(group => group.Count() == 3 || group.Count() == 2) == 2);
            // Kind of a nuisance because I separated variables. If I trimmed it down AND REMEMBERED *All()*, I'd save lots of space
            
            
            var fullHouses2 = pokerHands
                .Split(';')
                .Where(
                    hand => hand.Split(' ')
                        .GroupBy(card => card[0])
                        .All(g => g.Count() == 2 || g.Count() == 3) // This has an assumption that card hands are always 5.
                );                                              // Mine is longer because it checks there only TWO hands with 3 or 2. Not perfect but perhaps a little better
                    
            /////////////// QUESTION THREE: What day of the week is Christmas for next 10 years (starting with 2018)?
            var christmasDays = Enumerable.Range(0, 10)
                .Select(yearsFromNow => new DateTime(2018 + yearsFromNow, 12, 25).DayOfWeek);
            // Could actually do Enumerable.Range(2018, 10) and just have `years` instead of `2018 + yearsFromNow`
            
            /////////////// QUESTION FOUR: anagrams of "star" only
            var anagramWords = "parts,traps,arts,rats,starts,tarts,rat,art,tar,tars,stars,stray";
            var anagrams = anagramWords
                .Split(',')
                .Where(word => word.Length == 4 && "star".All(c => word.Count(c2 => c2 == c) == 1));
            
            // Answer, if you realise you could just sort the chars in each word - "star" --> "arst"
            var anagrams2 = anagramWords
                .Split(',')
                .Where(word => string.Concat(word.OrderBy(c => c)) == "arst");
            
            // NOTE: you can make this more readable with a local function:
            string Sort(string s) => new string(s.OrderBy(c => c).ToArray());
            anagrams2 = anagramWords
                .Split(',')
                .Where(word => Sort(word) == Sort("star"));

            //////////////// QUESTION FIVE: Find groups of names with matching initials
            var peopleNames =
                "Santi Cazorla, Per Mertesacker, Alan Smith, Thierry Henry, Alex Song, Paul Merson, Alexis Sánchez, Robert Pires, Dennis Bergkamp, Sol Campbell";
            var matchingInitials = Puzzle3_5FindMatchingInitials(peopleNames);

            var matchingInitials2 = peopleNames
                .Split(", ")
                .GroupBy(fullName => string.Join("", fullName.Split(' ').Select(name => name[0])))
                .Where(g => g.Count() > 1); // If we only want the matching initial groups, not all groups
            
            
            ////////////// QUESTION SIX: Video Editing - turn times to chop --> times to keep (of 2hr footage)
            var timesToChop = "0:00:00-0:00:05;0:55:12-1:05:02;1:37:47-1:37:51";
            // TODO: finish Question 6
        }

        /// Puzzle #3 Question 5 - Return groups of people with matching initial TODO: Return initials or original names?
        private static IEnumerable<IGrouping<string, string>> Puzzle3_5FindMatchingInitials(string peopleNames)
        {
            var matchingInitials = peopleNames
                .Split(", ") // comma AND whitespace, else trim whitespace later.
                .GroupBy(fullName => new string
                    (
                        fullName.Split(' ').Select(name => name.First()).ToArray()
                    )
                )
                .Where(g => g.Count() > 1);
            return matchingInitials;
        }
    }
}
