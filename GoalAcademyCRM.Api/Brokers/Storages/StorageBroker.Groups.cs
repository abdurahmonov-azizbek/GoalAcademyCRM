// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using GoalAcademyCRM.Api.Models.Groups;
using Microsoft.EntityFrameworkCore;

namespace GoalAcademyCRM.Api.Brokers.Storages
{
    public partial class StorageBroker
    {
        public DbSet<Group> Groups { get; set; }

        public async ValueTask<Group> InsertGroupAsync(Group group) =>
            await InsertAsync(group);

        public IQueryable<Group> SelectAllGroups() =>
            SelectAll<Group>();

        public async ValueTask<Group> SelectGroupByIdAsync(Guid groupId) =>
            await SelectAsync<Group>(groupId);

        public async ValueTask<Group> DeleteGroupAsync(Group group) =>
            await DeleteAsync(group);

        public async ValueTask<Group> UpdateGroupAsync(Group group) =>
            await UpdateAsync(group);
    }
}