using System.Threading.Tasks;

namespace TaskDistributionSystem.Services
{
    public interface IAssignmentService
    {
        Task AssignTaskAsync(int gorevId);
    }
}
