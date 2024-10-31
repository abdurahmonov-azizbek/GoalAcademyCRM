// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using GoalAcademyCRM.Api.Brokers.DateTimes;
using GoalAcademyCRM.Api.Brokers.Loggings;
using GoalAcademyCRM.Api.Brokers.Storages;
using GoalAcademyCRM.Api.Models.StudentGroups;

namespace GoalAcademyCRM.Api.Services.Foundations.StudentGroups
{
    public partial class StudentGroupService : IStudentGroupService
    {

        private readonly IStorageBroker storageBroker;
        private readonly IDateTimeBroker dateTimeBroker;
        private readonly ILoggingBroker loggingBroker;

        public StudentGroupService(
            IStorageBroker storageBroker,
            IDateTimeBroker dateTimeBroker,
            ILoggingBroker loggingBroker)

        {
            this.storageBroker = storageBroker;
            this.dateTimeBroker = dateTimeBroker;
            this.loggingBroker = loggingBroker;
        }

        public ValueTask<StudentGroup> AddStudentGroupAsync(StudentGroup studentGroup) =>
        TryCatch(async () =>
        {
            ValidateStudentGroupOnAdd(studentGroup);

            return await this.storageBroker.InsertStudentGroupAsync(studentGroup);
        });

        public IQueryable<StudentGroup> RetrieveAllStudentGroups() =>
            TryCatch(() => this.storageBroker.SelectAllStudentGroups());

        public ValueTask<StudentGroup> RetrieveStudentGroupByIdAsync(Guid studentGroupId) =>
           TryCatch(async () =>
           {
               ValidateStudentGroupId(studentGroupId);

               StudentGroup maybeStudentGroup =
                   await storageBroker.SelectStudentGroupByIdAsync(studentGroupId);

               ValidateStorageStudentGroup(maybeStudentGroup, studentGroupId);

               return maybeStudentGroup;
           });

        public ValueTask<StudentGroup> ModifyStudentGroupAsync(StudentGroup studentGroup) =>
            TryCatch(async () =>
            {
                ValidateStudentGroupOnModify(studentGroup);

                StudentGroup maybeStudentGroup =
                    await this.storageBroker.SelectStudentGroupByIdAsync(studentGroup.Id);

                ValidateAgainstStorageStudentGroupOnModify(inputStudentGroup: studentGroup, storageStudentGroup: maybeStudentGroup);

                return await this.storageBroker.UpdateStudentGroupAsync(studentGroup);
            });

        public ValueTask<StudentGroup> RemoveStudentGroupByIdAsync(Guid studentGroupId) =>
           TryCatch(async () =>
           {
               ValidateStudentGroupId(studentGroupId);

               StudentGroup maybeStudentGroup =
                   await this.storageBroker.SelectStudentGroupByIdAsync(studentGroupId);

               ValidateStorageStudentGroup(maybeStudentGroup, studentGroupId);

               return await this.storageBroker.DeleteStudentGroupAsync(maybeStudentGroup);
           });
    }
}