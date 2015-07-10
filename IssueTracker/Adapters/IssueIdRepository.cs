using System.Threading;
using System.Threading.Tasks;
using IssueTracker.Services;
using IssueTracker.Services.Models;

namespace IssueTracker.Adapters
{
    internal class IssueIdRepository : IssueCreationService.IGetEventIds
    {
        private readonly DocumentDbAdapter<IssueId> _docDbAdapter = new DocumentDbAdapter<IssueId>();
        private static readonly SemaphoreSlim Locker = new SemaphoreSlim(1);

        public async Task<int> GetNextId()
        {
            var issueId = 1;
            try
            {
                await Locker.WaitAsync();

                await _docDbAdapter.GetItem(id => true,
                    () => _docDbAdapter.AddItem(new IssueId {CurrentIssueId = 1}),
                    item =>
                    {
                        item.CurrentIssueId++;
                        issueId = item.CurrentIssueId;
                        return _docDbAdapter.UpdateItem(item);
                    });

                return issueId;
            }
            finally
            {
                Locker.Release();
            }
        }
    }
}