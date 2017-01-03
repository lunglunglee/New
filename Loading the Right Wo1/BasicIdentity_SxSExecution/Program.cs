using System;

namespace BasicIdentity_SxSExecution
{

    class Program
    {
        static void Main(string[] args)
        {
            bool exit = false;

            while (!exit)
            {
                Utils.ShowMessageFromHost("1. Illustrate side-by-side execution using basic identity");
                Utils.ShowMessageFromHost("2. Exit");

                string option = Console.ReadLine();

                switch (option)
                {
                    case "1":
                        SxSExecution.RunScenario();
                        break;
                    case "2":
                        exit = true;
                        break;
                    default:
                        Utils.ShowMessageFromHost("Unknown option", ConsoleColor.Red);
                        break;
                }
            }

            return;
 
        }
    }
}
