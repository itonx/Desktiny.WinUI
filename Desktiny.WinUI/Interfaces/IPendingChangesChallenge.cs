using System.Threading.Tasks;

namespace Desktiny.WinUI.Interfaces
{
    public interface IPendingChangesChallenge
    {
        Task<bool> DiscardChangesAsync(bool completeChallenge = false);
        bool ChallengeCompleted { get; }
    }
}
