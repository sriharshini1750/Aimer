using Aimer.Console.Actions;
using Aimer.Console.IActions;

namespace Aimer.Console;

public class App(IEnumerable<IAction> actions)
{
    private readonly IReadOnlyList<IAction> _actions = actions.ToList();

    public async Task RunAsync()
    {
        while (true)
        {
            System.Console.WriteLine();
            for (var i = 0; i < _actions.Count; i++)
                System.Console.WriteLine($"{i + 1}. {_actions[i].Title}");
            System.Console.WriteLine("0. Exit");
            System.Console.Write("> ");

            var input = System.Console.ReadLine()?.Trim();
            if (input == "0") return;

            if (int.TryParse(input, out var choice) && choice >= 1 && choice <= _actions.Count)
                await RunSafelyAsync(_actions[choice - 1]);
            else
                System.Console.WriteLine("Invalid choice.");
        }
    }

    private static async Task RunSafelyAsync(IAction action)
    {
        try
        {
            System.Console.WriteLine($"Running action: {action}");
            
            await action.RunAsync();
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"💥 {action.Title} failed: {ex.Message}");
        }
    }
}