using Orbital7.Extensions.ScriptJobs;
using ScriptJobsConsole.ScriptJobs;
using System;

namespace ScriptJobsConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            ScriptJobExecutionEngine.Execute(
               new CreateTestWeb());
        }
    }
}
