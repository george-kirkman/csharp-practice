using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleAsync
{
    static class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var num1 = 5.5;
            var num2 = 6.0;
            
            // Slow multiplication
            Console.WriteLine($"Computing {num1} * {num2}...");
            var productTask = SlowMultiply(num1, num2);
            
            // Fast addition not blocked by slow multiplication
            Console.WriteLine($"Computing {num1} + {num2}...");
            var sumTask = QuickAdd(num1, num2);

            // Simple approach, tasks are not blocked but results are accessed *synchronously*
            // Benefits: simple code, time-consuming tasks start & run without blocking each other
            // Costs: In this case, sum is way faster than product, but sum prints after product, aka after long delay
            //        even though its task completed ages ago.
            var product = await productTask;
            var sum = await sumTask;
            
            // Write the appropriate result to console when it completes.
            // * Is there a better/functional way to do this without while/if?
            var mathTasks = new List<Task> {productTask, sumTask};
            while (mathTasks.Count > 0)
            {
                var finishedTask = await Task.WhenAny(mathTasks);
                if (finishedTask == productTask)
                {
                    Console.WriteLine($"{num1} * {num2} = {productTask.Result}");
                }
                else if (finishedTask == sumTask)
                {
                    Console.WriteLine($"{num1} + {num2} = {sumTask.Result}");
                }

                mathTasks.Remove(finishedTask);
            }
        }

        static async Task<double> SlowMultiply(double num1, double num2)
        {
            await Task.Delay(2000);
            return num1 * num2;
        }

        static async Task<double> QuickAdd(double num1, double num2)
        {
            await Task.Delay(10);
            return num1 + num2;
        }
    }
}