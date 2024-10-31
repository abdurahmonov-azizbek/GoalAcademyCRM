// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using GoalAcademyCRM.Api.Models.Groups;

namespace GoalAcademyCRM.Api.Services.Foundations.Groups
{
    public interface IGroupService  
    {
        /// <exception cref="Models.Groups.Exceptions.GroupValidationException"></exception>
        /// <exception cref="Models.Groups.Exceptions.GroupDependencyValidationException"></exception>
        /// <exception cref="Models.Groups.Exceptions.GroupDependencyException"></exception>
        /// <exception cref="Models.Groups.Exceptions.GroupServiceException"></exception>
        ValueTask<Group> AddGroupAsync(Group group);

        /// <exception cref="Models.Groups.Exceptions.GroupDependencyException"></exception>
        /// <exception cref="Models.Groups.Exceptions.GroupServiceException"></exception>
        IQueryable<Group> RetrieveAllGroups();

        /// <exception cref="Models.Groups.Exceptions.GroupDependencyException"></exception>
        /// <exception cref="Models.Groups.Exceptions.GroupServiceException"></exception>
        ValueTask<Group> RetrieveGroupByIdAsync(Guid groupId);

        /// <exception cref="Models.Groups.Exceptions.GroupValidationException"></exception>
        /// <exception cref="Models.Groups.Exceptions.GroupDependencyValidationException"></exception>
        /// <exception cref="Models.Groups.Exceptions.GroupDependencyException"></exception>
        /// <exception cref="Models.Groups.Exceptions.GroupServiceException"></exception>
        ValueTask<Group> ModifyGroupAsync(Group group);

        /// <exception cref="Models.Groups.Exceptions.GroupDependencyValidationException"></exception>
        /// <exception cref="Models.Groups.Exceptions.GroupDependencyException"></exception>
        /// <exception cref="Models.Groups.Exceptions.GroupServiceException"></exception>
        ValueTask<Group> RemoveGroupByIdAsync(Guid groupId);
    }
}