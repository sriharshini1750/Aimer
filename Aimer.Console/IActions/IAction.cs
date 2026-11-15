using System.Threading;
using System.Threading.Tasks;

namespace Aimer.Console.IActions
{
    public interface IAction
    {string Title { get; }
         public Task RunAsync(CancellationToken cancellationToken = default);
    }
}