using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Optional;
using Optional.Collections;

// FOLLOWING README FROM https://github.com/nlkl/Optional
namespace Optionals
{
    class Program
    {
        static void Main(string[] args)
        {
            // Bit of background on what nullable type is
            int? test = null; // Can be null
            int test1 = 2;    // Cannot be null
            var test2 = test1 + test; // Becomes nullable type "int?"
            Console.WriteLine(test2); // Prints nothing
            
            
            // Basic create Option
            var noneInt = Option.None<int>();
            var someInt = Option.Some(10);
            // Shorthand
            noneInt = 10.None();
            someInt = 10.Some();
            Console.WriteLine(noneInt);
            Console.WriteLine(someInt);

            // SomeNotNull() - filters out Null values
            string nullString = null;
            var noneString = nullString.SomeNotNull();
            Console.WriteLine(noneString);
            
            // SomeWhen() - returns some only when predicate satisfied
            string str = "abc";
            noneString = str.SomeWhen(s => s == "cba"); // Return None if predicate is violated
            noneString = str.NoneWhen(s => s == "abc"); // Return None if predicate is satisfied
            
            // ToOption() - converts Nullable type (first class) to Optional (NuGet package)
            int? nullableWithoutValue = null;
            int? nullableWithValue = 2;
            noneInt = nullableWithoutValue.ToOption();
            someInt = nullableWithValue.ToOption();
            
            /*
             * RETRIEVING VALUES
             */
            var hasValue = someInt.HasValue; // True if a value is present. False if Option is none.
            var isThousand = someInt.Contains(1000); // True if option contains specific value. False if not that value (or is none)
            var isGreaterThanThousand = someInt.Exists(x => x > 1000); // True if value satisfies predicate. False if does not (or is none)
            
            // ValueOr() - returns unwrapped value, or provides alternative value if None
            var value = noneInt.ValueOr(10);

            // Match() - similar to valueOr, but performs some lambda operation on the value (or on alternative)
            value = noneInt.Match(xSome => xSome+10, () => 10);  // Why isn't this ok if I name xNone instead of ()?
            
            /*
             * MANIPULATING OPTIONAL VALUES
             */
            someInt = noneInt.Or(10); // Or() provides a *some* alternative if *none*. DOES NOT return unwrapped value like ValueOr().
            someInt = noneInt.Else(10.Some()); // Else() provides an alternative Option if *none*.
            noneInt = noneInt.Else(Option.None<int>()); // NOTE: Else() might not always provide a *Some* return like Or() does.


            // Map() - transforms value IF some, if none then it just stays none
            var none = Option.None<int>();
            var stillNone = none.Map(x => x + 10);
            
            var some = 10.Some();
            var somePlus10 = some.Map(x => x + 10);
            
            // FlatMap() - is similar, but transformation return type *must* be another optional.
            // its purpose is to stop Map() from giving you nested optionals, if your transformation returns an optional of the optional you're transforming.
            var optional = 10.Some();
            var nestedOptional = optional.Map(x => x.Some());
            var flattenedOptional = optional.FlatMap(x => x.Some());
            // Note: if you DO end up with a nested optional, you can use Flatten()
            flattenedOptional = nestedOptional.Flatten();
            
            // Filter() - returns None if filter predicate not satisfied.
            someInt = 10.Some();
            var stillSomeInt = someInt.Filter(x => x == 10); // Predicate true, returns same as before
            noneInt = someInt.Filter(x => x == 999);                   // Predicate false, returns None optional
            noneInt = noneInt.Filter(x => x == 10);                    // Of course, None still returns None

            // NOTE: NotNull() - We commonly want to filter away null values. There's a specific filter just for that.
            // var parent = GetNode()
            //     .Map(node => node.Parent)
            //     .NotNull(); 
            
            // Options can be treated as a *Collection* of either 1 or no elements
            foreach (var val in someInt)
            {
                Console.WriteLine(val);
            }
            
            // Options can be made into Enumerables if you want to use Linq stuff (?)
            var enumerable = someInt.ToEnumerable();
            
            /* Two optional values are equal if the following is satisfied:
             *     The two options have the same type
             *     Both are none, both contain null values, or the contained values are equal
             * Read more: https://github.com/nlkl/Optional#equivalence-and-comparison
             */

            /*
             * Working with COLLECTIONS! WOOOOO LINQ!
             */
            // These are LINQ expressions that could return NULL
            var collection = Enumerable.Range(0, 0);
            var nullable = collection.FirstOrDefault();
            nullable = collection.LastOrDefault();
            nullable = collection.SingleOrDefault();
            Console.WriteLine($"Nullable value = {nullable}"); // "Nullable value = 0 !!! Not good! NOTE: Removing "OrDefault" results in exceptions instead.
            nullable = collection.ElementAtOrDefault(index: 5);

            // Let's be sensible and return Optionals!
            var option = collection.FirstOrNone();
            option = collection.LastOrNone();
            option = collection.SingleOrNone();
            option = collection.ElementAtOrNone(5);
            
            // This is extra useful when using predicates in the above selectors - you might not know if they'll be satisfied at all.
            option = collection.FirstOrNone(x => x == 5);
            option = collection.LastOrNone(x => x == 5);
            option = collection.SingleOrNone(x => x == 5); // This makes sure that only *one* element in the collection satisfies the predicate.
            Console.WriteLine($"Better, optional value = {option} (it's an option)");
            
            // George playing around
            var twoFives = new List<int>{ 1,2,3,4,5,5,6,7,8,9 };
            var shouldBeSome = twoFives.SingleOrNone(x => x == 2);
            var shouldBeNone = twoFives.SingleOrNone(x => x == 5); // SingleOrDefault throws an EXCEPTION!
            Console.WriteLine(shouldBeSome);
            Console.WriteLine(shouldBeNone);
            
            Example();
        }

        static void Example()
        {
            List<int> numbers = new List<int>() {1, 2, 3, 4, 4, 5, 6, 7, 7, 8, 9};
            IEnumerable<Option<int>> indexIsSingle = numbers
                .Select(x => numbers.SingleOrNone(y => y == x));
            Console.WriteLine($"indexIsSingle: {string.Join(',', indexIsSingle)}");

            // How to use .Values()
            IEnumerable<int> onlyValuesNoNones = indexIsSingle.Values();
            Console.WriteLine($"Using collection.Values(): {string.Join(',', onlyValuesNoNones)}");

            // .Values() but manually (DO NOT USE! Just for understanding)
            onlyValuesNoNones = indexIsSingle
                .Where(x => x.HasValue)
                .Select(x => x.ValueOr(0));
            Console.WriteLine($"manual .Values(): {string.Join(',', onlyValuesNoNones)}");
            
            // Return list values if no Nones, else return None.
            
            // WRITING IN CONVENTIONAL TEMP
            var forTestList = new List<int>();
            foreach (var x in indexIsSingle)
            {
                if (x.HasValue)
                {
                    forTestList.Add(x.ValueOr(0));
                }
                else
                {
                    forTestList.Clear();
                    break;
                }
            }

            // Immutable list always!
            var listOfSomesAndNones = indexIsSingle.ToImmutableList();
            
            IEnumerable<Option<int>> listOfSomes = Enumerable.Range(0, 10)
                .Select(x => Enumerable.Range(0,10).SingleOrNone(y => y == x))
                .ToImmutableList();
            // Attempt
            var valuesOrNone = listOfSomesAndNones
                .Aggregate(ImmutableList.Create<int>(),
                    (acc, next) => next.HasValue ? acc.Add(next.ValueOr(0)) : acc.Clear()); // Add to list only if Some. If Option to add is None, scrap acc and don't add.
            // Problem - if there is a None in the middle of list, will return the rest of the values after it.
            // How to break an aggregate if condition is met?
            
            // Attempt2, too many iterations
            var valuesOrNone3 = indexIsSingle.All(x => x.HasValue) ? indexIsSingle.Values() : new List<int>();
            
            
            var valuesOrNoneAnswer = listOfSomes
                .Aggregate(ImmutableList<int>.Empty.None(),
                    (acc, next) => acc.FlatMap(list => next.Map(n => list.Add(n))));

            // NOTE: MAP CAN RETURN OPTIONS OF DIFFERENT TYPES TO WHAT IT'S MAPPING!
            ImmutableList<int> list = ImmutableList<int>.Empty;
            Option<int> next = 10.Some();
            var result = next.Map(n => list.Add(n));    // Next is Option<int>, result is Option<List<int>>
        }
    }
}