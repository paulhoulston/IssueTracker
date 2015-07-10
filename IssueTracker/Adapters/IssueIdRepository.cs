using System.Threading;
using System.Threading.Tasks;
using IssueTracker.Services;

namespace IssueTracker.Adapters
{
    internal class IssueIdRepository : IssueCreationService.IGetEventIds
    {
        private readonly DocumentDbAdapter<IssueId> _docDbAdapter = new DocumentDbAdapter<IssueId>();
        private static readonly SemaphoreSlim Locker = new SemaphoreSlim(1);

        [DocumentCollectionId("IssueIds")]
        public class IssueId : IDocumentItem
        {
            public string Id { get; set; }
            public int CurrentIssueId { get; set; }
        }

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