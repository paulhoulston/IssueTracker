using System;
using System.Threading.Tasks;
using IssueTracker.Services;
using IssueTracker.Services.Models;
using NUnit.Framework;

namespace IssueTracker.Tests
{
    internal class Given_I_want_to_create_an_issue
    {
        private class When_I_create_an_issue : IssueCreationService.ICreateIssues, IssueCreationService.IGetEventIds
        {
            private readonly DateTime _testStartTime = DateTime.UtcNow;
            private Issue _issue;
            private const int ExpectedIssueId = 1;
            private const string CreatedBy = "Paul Houslton";

            [TestFixtureSetUp]
            public void SetUp()
            {
                Task.Run(async () => await new IssueCreationService(this, this).CreateIssue(CreatedBy, issue => _issue = issue));
            }

            [Test]
            public void Then_the_issue_is_created()
            {
                Assert.IsNotNull(_issue);
            }

            [Test]
            public void And_the_creator_is_assigned_to_the_event()
            {
                Assert.AreEqual(CreatedBy, _issue.CreatedBy);
            }

            [Test]
            public void And_the_current_UTC_timestamp_is_added_to_the_event()
            {
                Assert.GreaterOrEqual(_issue.CreatedTime, _testStartTime);
            }

            [Test]
            public void And_the_expected_ID_is_assigned_to_an_event()
            {
                Assert.AreEqual(ExpectedIssueId, _issue.IssueId);
            }

            public async Task CreateIssue(Issue issue)
            {
                //nothing to do.
            }

            public async Task<int> GetNextId()
            {
                return ExpectedIssueId;
            }
        }
    }
}