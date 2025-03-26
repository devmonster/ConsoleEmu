using System.Text;
using System.Text.Json;

namespace ConsoleEmu;

public class Configuration
{
    public GeneralConfiguration Config { get; set; }
    public List<ScenarioItem> Scenarios { get; set; }
}


public class GeneralConfiguration
{
    public string Prompt { get; set; }
    public string[] Headers { get; set; }
    public bool ClearOnStart { get; set; } = false;
    public string Title { get; set; }
}

public class ScenarioItem
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
    public string[] Output { get; set; }
    public int Delay { get; set; } = 400; // Default delay between lines0
    public string? Color { get; set; }

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
            var configuration = JsonSerializer.Deserialize<Configuration>(jsonContent,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            if (configuration.Config.ClearOnStart) Console.Clear();

            if (configuration.Config.Headers?.Length > 0)
            {
                foreach (string header in configuration.Config.Headers) Console.WriteLine(header);
                Console.WriteLine();
            }

            if (!string.IsNullOrEmpty(configuration.Config.Title)) Console.Title = configuration.Config.Title;

            // Run each scenario
            foreach (var scenario in configuration.Scenarios)
            {
                await RunInteractiveScenario(scenario, configuration.Config.Prompt);
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

    static async Task RunInteractiveScenario(ScenarioItem scenario, string prompt)
    {

        // Prepare for interactive input
        string expectedInput = scenario.Input;
        StringBuilder currentInput = new StringBuilder();

        Console.Write(prompt);

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



        foreach (var outputItem in outputs)
        {
            if (outputItem.Color is not null)
                Console.ForegroundColor = GetNamedColorFromString(outputItem.Color);


            foreach (var outputline in outputItem.Output)
            {
                Console.WriteLine(outputline);
            }

            await Task.Delay(outputItem.Delay);



        }
        Console.ResetColor();
    }

    static ConsoleColor GetNamedColorFromString(string color)
    {
        return color.ToLower() switch
        {
            "black" => ConsoleColor.Black,
            "darkblue" => ConsoleColor.DarkBlue,
            "darkgreen" => ConsoleColor.DarkGreen,
            "darkcyan" => ConsoleColor.DarkCyan,
            "darkred" => ConsoleColor.DarkRed,
            "darkmagenta" => ConsoleColor.DarkMagenta,
            "darkyellow" => ConsoleColor.DarkYellow,
            "gray" => ConsoleColor.Gray,
            "darkgray" => ConsoleColor.DarkGray,
            "blue" => ConsoleColor.Blue,
            "green" => ConsoleColor.Green,
            "cyan" => ConsoleColor.Cyan,
            "red" => ConsoleColor.Red,
            "magenta" => ConsoleColor.Magenta,
            "yellow" => ConsoleColor.Yellow,
            "white" => ConsoleColor.White,
            _ => throw new ArgumentException($"Invalid color name: {color}")
        };
    }

}


