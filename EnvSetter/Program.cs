using System;
using System.IO;

namespace EnvSetter
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            // if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("NEW_WB")))
            // {
                Environment.SetEnvironmentVariable("NEW_WB", currentDirectory, EnvironmentVariableTarget.Machine);
            // }
        }
    }
}