using System.Threading;
using System.Threading.Tasks;
using IssueTracker.Services;

namespace IssueTracker.Adapters
{
    internal class IssueIdRepository : IssueCreationService.IGetEventIds
    {
        private const string CollectionId = "IssueIds";
        private readonly DocumentDbAdapter<IssueId> _docDbAdapter = new DocumentDbAdapter<IssueId>();
        private static readonly SemaphoreSlim Locker = new SemaphoreSlim(1);

        public class IssueId : IDocumentItem
        {
            public string Id { get; set; }
            public int CurrentIssueId { get; set; }
        }

        public async Task<int> GetNextId()
        {
            try
            {
                await Locker.WaitAsync();

                var item = await _docDbAdapter.GetItem(CollectionId, id => true);
                if (item == null)
                {
                    item = new IssueId {CurrentIssueId = 1};
                    await _docDbAdapter.AddItem(item, CollectionId);
                }
                else
                {
                    item.CurrentIssueId++;
                    await _docDbAdapter.UpdateItem(CollectionId, item);
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