// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using GoalAcademyCRM.Api.Models.Groups;

namespace GoalAcademyCRM.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Group> InsertGroupAsync(Group group);
        IQueryable<Group> SelectAllGroups();
        ValueTask<Group> SelectGroupByIdAsync(Guid groupId);
        ValueTask<Group> DeleteGroupAsync(Group group);
        ValueTask<Group> UpdateGroupAsync(Group group);
    }
}