using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsyncBreakfast
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Coffee cup = PourCoffee();
            Console.WriteLine("coffee is ready");
            
            Task<Egg> eggsTask = FryEggsAsync(2);
            
            // Joseph asked "How do I now start 50 of these tasks at the same time?
            var eggTasks = Enumerable.Repeat<object>(null, 50).Select(_ => FryEggsAsync(2));
            
            // HOWEVER!!! Select is *LAZY* - Those tasks will not start until we EVALUATE the Select vvvvvv
            var friedEggs = Task.WhenAll(eggTasks);
            var friedEggsAlternateEvaluate = eggTasks.ToList(); // We could just throw this in the eggTasks initialisation anyway.
            //End of experiments
            
            Task<Bacon> baconTask = FryBaconAsync(3);
            //Task<Toast> toastTask = ToastBreadAsync(2);
            var toastTask = MakeToastWithButterAndJamAsync(2);

            /*
            Egg eggs = await eggsTask;
            Console.WriteLine("eggs are ready");
            
            //Bacon bacon = await baconTask;
            Console.WriteLine("bacon is ready");
            
            //Toast toast = await toastTask;
            //ApplyButter(toast);   //Since these follow the toast await, better to move it inside that function.
            //ApplyJam(toast);
            Console.WriteLine("toast is ready");
            */

            // Instead using Task.WhenAny to fire any time an awaitable task in the list finishes
            // Note: after each task completes, we remove it from the list so to only check remaining tasks.
            var breakfastTasks = new List<Task> {eggsTask, baconTask, toastTask };
            while (breakfastTasks.Count > 0)
            {
                Task finishedTask = await Task.WhenAny(breakfastTasks);
                if (finishedTask == eggsTask)
                {
                    Console.WriteLine("eggs are ready");
                }
                else if (finishedTask == baconTask)
                {
                    Console.WriteLine("bacon is ready");
                } else if (finishedTask == toastTask)
                {
                    Console.WriteLine("toast is ready");
                }
                breakfastTasks.Remove(finishedTask);
            }
            Juice oj = PourOJ();
            Console.WriteLine("oj is ready");
            // await Task.WhenAll(eggsTask, baconTask, toastTask);
            Console.WriteLine("Breakfast is ready!");
        }

        private static Juice PourOJ()
        {
            Console.WriteLine("Pouring orange juice");
            return new Juice();
        }

        private static void ApplyJam(Toast toast) => 
            Console.WriteLine("Putting jam on the toast");

        private static void ApplyButter(Toast toast) => 
            Console.WriteLine("Putting butter on the toast");

        /*
         * OLD SYNCHRONOUS TASKS. BAD! ANY SLOW OPERATIONS BLOCK THE WHOLE THREAD
         */
        private static Toast ToastBread(int slices)
        {
            for (int slice = 0; slice < slices; slice++)
            {
                Console.WriteLine("Putting a slice of bread in the toaster");
            }
            Console.WriteLine("Start toasting...");
            Task.Delay(3000).Wait();
            Console.WriteLine("Remove toast from toaster");

            return new Toast();
        }

        private static Bacon FryBacon(int slices)
        {
            Console.WriteLine($"putting {slices} slices of bacon in the pan");
            Console.WriteLine("cooking first side of bacon...");
            Task.Delay(3000).Wait();
            for (int slice = 0; slice < slices; slice++)
            {
                Console.WriteLine("flipping a slice of bacon");
            }
            Console.WriteLine("cooking the second side of bacon...");
            Task.Delay(3000).Wait();
            Console.WriteLine("Put bacon on plate");

            return new Bacon();
        }

        private static Egg FryEggs(int howMany)
        {
            Console.WriteLine("Warming the egg pan...");
            Task.Delay(3000).Wait();
            Console.WriteLine($"cracking {howMany} eggs");
            Console.WriteLine("cooking the eggs ...");
            Task.Delay(3000).Wait();
            Console.WriteLine("Put eggs on plate");

            return new Egg();
        }

        private static Coffee PourCoffee()
        {
            Console.WriteLine("Pouring coffee");
            return new Coffee();
        }
        
        /*
         * ASYNCHRONOUS TASKS. CODE THAT CALLS IT GETS GIVEN AN AWAITABLE TASK AND IMMEDIATELY
         * MOVES ON TO THE NEXT LINE OF CODE. 
         */
       //Since there are sync tasks that follow this async task, this method is updated below
       //to include those synchronous tasks in the same method.
        private static async Task<Toast> ToastBreadAsync(int slices)
        {
            for (int slice = 0; slice < slices; slice++)
            {
                Console.WriteLine("Putting a slice of bread in the toaster");
            }
            Console.WriteLine("Start toasting...");
            await Task.Delay(3000);
            //Console.WriteLine("FIRE! Toast is ruined!");
            //throw new InvalidOperationException("The toaster is on fire!");
            Console.WriteLine("Remove toast from toaster");

            return new Toast();
        }

        // Async task followed by SYNC tasks are COLLECTIVELY all async.
        // Smarter to group the sync stuff that follows async into the function.
        static async Task<Toast> MakeToastWithButterAndJamAsync(int number)
        {
            var toast = await ToastBreadAsync(number);
            ApplyButter(toast);
            ApplyJam(toast);

            return toast;
        }

        //Async keyword by itself doesn't do anything except enabling you to use the keyword await within the method.
        private static async Task<Bacon> FryBaconAsync(int slices)
        {
            Console.WriteLine($"putting {slices} slices of bacon in the pan");
            Console.WriteLine("cooking first side of bacon...");
            await Task.Delay(3000);
            for (int slice = 0; slice < slices; slice++)
            {
                Console.WriteLine("flipping a slice of bacon");
            }
            Console.WriteLine("cooking the second side of bacon...");
            await Task.Delay(3000);
            Console.WriteLine("Put bacon on plate");

            return new Bacon();
        }

        private static async Task<Egg> FryEggsAsync(int howMany)
        {
            Console.WriteLine("Warming the egg pan...");
            await Task.Delay(3000);
            Console.WriteLine($"cracking {howMany} eggs");
            Console.WriteLine("cooking the eggs ...");
            await Task.Delay(3000);
            Console.WriteLine("Put eggs on plate");
            
            return new Egg();
        }
    }
    
    

    internal class Juice
    {
    }

    internal class Toast
    {
    }

    internal class Bacon
    {
    }

    internal class Egg
    {
    }

    internal class Coffee
    {
    }
}