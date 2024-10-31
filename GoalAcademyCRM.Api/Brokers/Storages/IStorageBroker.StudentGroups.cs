// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using GoalAcademyCRM.Api.Models.StudentGroups;

namespace GoalAcademyCRM.Api.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<StudentGroup> InsertStudentGroupAsync(StudentGroup studentGroup);
        IQueryable<StudentGroup> SelectAllStudentGroups();
        ValueTask<StudentGroup> SelectStudentGroupByIdAsync(Guid studentGroupId);
        ValueTask<StudentGroup> DeleteStudentGroupAsync(StudentGroup studentGroup);
        ValueTask<StudentGroup> UpdateStudentGroupAsync(StudentGroup studentGroup);
    }
}