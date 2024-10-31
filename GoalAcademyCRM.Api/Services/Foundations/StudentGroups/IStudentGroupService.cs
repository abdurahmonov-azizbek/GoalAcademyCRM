// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using GoalAcademyCRM.Api.Models.StudentGroups;

namespace GoalAcademyCRM.Api.Services.Foundations.StudentGroups
{
    public interface IStudentGroupService  
    {
        /// <exception cref="Models.StudentGroups.Exceptions.StudentGroupValidationException"></exception>
        /// <exception cref="Models.StudentGroups.Exceptions.StudentGroupDependencyValidationException"></exception>
        /// <exception cref="Models.StudentGroups.Exceptions.StudentGroupDependencyException"></exception>
        /// <exception cref="Models.StudentGroups.Exceptions.StudentGroupServiceException"></exception>
        ValueTask<StudentGroup> AddStudentGroupAsync(StudentGroup studentGroup);

        /// <exception cref="Models.StudentGroups.Exceptions.StudentGroupDependencyException"></exception>
        /// <exception cref="Models.StudentGroups.Exceptions.StudentGroupServiceException"></exception>
        IQueryable<StudentGroup> RetrieveAllStudentGroups();

        /// <exception cref="Models.StudentGroups.Exceptions.StudentGroupDependencyException"></exception>
        /// <exception cref="Models.StudentGroups.Exceptions.StudentGroupServiceException"></exception>
        ValueTask<StudentGroup> RetrieveStudentGroupByIdAsync(Guid studentGroupId);

        /// <exception cref="Models.StudentGroups.Exceptions.StudentGroupValidationException"></exception>
        /// <exception cref="Models.StudentGroups.Exceptions.StudentGroupDependencyValidationException"></exception>
        /// <exception cref="Models.StudentGroups.Exceptions.StudentGroupDependencyException"></exception>
        /// <exception cref="Models.StudentGroups.Exceptions.StudentGroupServiceException"></exception>
        ValueTask<StudentGroup> ModifyStudentGroupAsync(StudentGroup studentGroup);

        /// <exception cref="Models.StudentGroups.Exceptions.StudentGroupDependencyValidationException"></exception>
        /// <exception cref="Models.StudentGroups.Exceptions.StudentGroupDependencyException"></exception>
        /// <exception cref="Models.StudentGroups.Exceptions.StudentGroupServiceException"></exception>
        ValueTask<StudentGroup> RemoveStudentGroupByIdAsync(Guid studentGroupId);
    }
}