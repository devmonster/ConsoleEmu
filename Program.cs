using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsoleEmulator
{
    public class ConsoleScenario
    {
        public string Input { get; set; }
        public OutputStreamConfig OutputStream { get; set; }
    }

    public class OutputStreamConfig
    {
        public List<OutputItem> Outputs { get; set; }
    }

    public class OutputItem
    {
        public string Output { get; set; }
        public int Delay { get; set; } = 400; // Default delay between lines
    }

    public class InteractiveConsoleEmulator
    {
        static async Task Main(string[] args)
        {
            Console.Clear();

            try
            {
                // Read the configuration file
                string configPath = "scenario.json";
                string jsonContent = File.ReadAllText(configPath);

                // Deserialize the JSON
                var scenarios = JsonSerializer.Deserialize<List<ConsoleScenario>>(jsonContent,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                // Run each scenario
                foreach (var scenario in scenarios)
                {
                    await RunInteractiveScenario(scenario);
                    Console.WriteLine(); // Add a blank line between scenarios
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error: {ex.Message}");
                Console.ResetColor();
            }
        }

        static async Task RunInteractiveScenario(ConsoleScenario scenario)
        {
            
            
            Console.ForegroundColor = ConsoleColor.Green;

            // Prepare for interactive input
            string expectedInput = scenario.Input;
            StringBuilder currentInput = new StringBuilder();

            Console.Write("PS C:\\Demo> ");

            // Interactive input handling
            for (int i = 0; i < expectedInput.Length; i++)
            {
                // Wait for any key press
                Console.ReadKey(intercept: true);

                // Output the correct character
                Console.Write(expectedInput[i]);
                currentInput.Append(expectedInput[i]);
            }

            // Wait for Enter key
            while (Console.ReadKey(intercept: true).Key != ConsoleKey.Enter)
            {
                // Wait until Enter is pressed
            }

            Console.WriteLine();

            // Simulate output streams
            await SimulateOutputStreams(scenario.OutputStream?.Outputs);

            Console.ResetColor();
        }

        static async Task SimulateOutputStreams(List<OutputItem> outputs)
        {
            if (outputs == null) return;

            Console.ForegroundColor = ConsoleColor.Cyan;
            bool firstLine = true;
            foreach (var outputItem in outputs)
            {
                // Add a delay between lines (except for the first line)
                if (!firstLine)
                {
                    await Task.Delay(outputItem.Delay);
                }
                firstLine = false;

                // Output the entire line at once
                Console.WriteLine(outputItem.Output);
            }
            Console.ResetColor();
        }
    }
}