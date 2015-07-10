using System.Threading;
using System.Threading.Tasks;
using IssueTracker.Services;

namespace IssueTracker.Adapters
{
    internal class IssueIdRepository : IssueCreationService.IGetEventIds
    {
        private readonly DocumentDbAdapter<IssueId> _docDbAdapter = new DocumentDbAdapter<IssueId>();
        private static readonly SemaphoreSlim Locker = new SemaphoreSlim(1);

        public class IssueId
        {
            public string id { get; set; }
            public int CurrentIssueId { get; set; }
        }

        public async Task<int> GetNextId()
        {
            try
            {
                await Locker.WaitAsync();

                var item = await _docDbAdapter.GetItem("IssueIds", id => true);
                if (item == null)
                {
                    item = new IssueId {CurrentIssueId = 1};
                    await _docDbAdapter.AddItem(item, "IssueIds");
                }
                return item.CurrentIssueId;
            }
            finally
            {
                Locker.Release();
            }
        }
    }
}