// --------------------------------------------------------
// Copyright (c) Coalition of Good-Hearted Engineers
// Developed by abdurahmonov-azizbek
//  --------------------------------------------------------

using System;
using GoalAcademyCRM.Api.Models.Attendances;
using GoalAcademyCRM.Api.Models.Attendances.Exceptions;

namespace GoalAcademyCRM.Api.Services.Foundations.Attendances
{
    public partial class AttendanceService
    {
        private void ValidateAttendanceOnAdd(Attendance attendance)
        {
            ValidateAttendanceNotNull(attendance);

            Validate(
				(Rule: IsInvalid(attendance.Id), Parameter: nameof(Attendance.Id)),
				(Rule: IsInvalid(attendance.GroupId), Parameter: nameof(Attendance.GroupId)),
				(Rule: IsInvalid(attendance.StudentId), Parameter: nameof(Attendance.StudentId)),
				(Rule: IsInvalid(attendance.Reason), Parameter: nameof(Attendance.Reason)),
				(Rule: IsInvalid(attendance.CreatedDate), Parameter: nameof(Attendance.CreatedDate)),
				(Rule: IsInvalid(attendance.UpdatedDate), Parameter: nameof(Attendance.UpdatedDate)),

                (Rule: IsNotRecent(attendance.CreatedDate), Parameter: nameof(Attendance.CreatedDate)),

                (Rule: IsNotSame(
                    firstDate: attendance.CreatedDate,
                    secondDate: attendance.UpdatedDate,
                    secondDateName: nameof(Attendance.UpdatedDate)),

                    Parameter: nameof(Attendance.CreatedDate)));
        }

        private void ValidateAttendanceOnModify(Attendance attendance)
        {
            ValidateAttendanceNotNull(attendance);

            Validate(
				(Rule: IsInvalid(attendance.Id), Parameter: nameof(Attendance.Id)),
				(Rule: IsInvalid(attendance.GroupId), Parameter: nameof(Attendance.GroupId)),
				(Rule: IsInvalid(attendance.StudentId), Parameter: nameof(Attendance.StudentId)),
				(Rule: IsInvalid(attendance.Reason), Parameter: nameof(Attendance.Reason)),
				(Rule: IsInvalid(attendance.CreatedDate), Parameter: nameof(Attendance.CreatedDate)),
				(Rule: IsInvalid(attendance.UpdatedDate), Parameter: nameof(Attendance.UpdatedDate)),

                (Rule: IsNotRecent(attendance.UpdatedDate), Parameter: nameof(Attendance.UpdatedDate)),

                (Rule: IsSame(
                    firstDate: attendance.UpdatedDate,
                    secondDate: attendance.CreatedDate,
                    secondDateName: nameof(attendance.CreatedDate)),
                    Parameter: nameof(attendance.UpdatedDate)));
        }

        private static void ValidateAgainstStorageAttendanceOnModify(Attendance inputAttendance, Attendance storageAttendance)
        {
            ValidateStorageAttendance(storageAttendance, inputAttendance.Id);
            Validate(
            (Rule: IsNotSame(
                    firstDate: inputAttendance.CreatedDate,
                    secondDate: storageAttendance.CreatedDate,
                    secondDateName: nameof(Attendance.CreatedDate)),
                    Parameter: nameof(Attendance.CreatedDate)),

                     (Rule: IsSame(
                        firstDate: inputAttendance.UpdatedDate,
                        secondDate: storageAttendance.UpdatedDate,
                        secondDateName: nameof(Attendance.UpdatedDate)),
                        Parameter: nameof(Attendance.UpdatedDate)));
        }

        private static void ValidateStorageAttendance(Attendance maybeAttendance, Guid attendanceId)
        {
            if (maybeAttendance is null)
            {
                throw new NotFoundAttendanceException(attendanceId);
            }
        }

        private void ValidateAttendanceId(Guid attendanceId) =>
             Validate((Rule: IsInvalid(attendanceId), Parameter: nameof(Attendance.Id)));

        private void ValidateAttendanceNotNull(Attendance attendance)
        {
            if (attendance is null)
            {
                throw new NullAttendanceException();
            }
        }

        private static dynamic IsInvalid(Guid id) => new
        {
            Condition = id == Guid.Empty,
            Message = "Id is required"
        };

        private static dynamic IsNotSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate != secondDate,
                Message = $"Date is not same as {secondDateName}"
            };

        private dynamic IsInvalid(string text) => new
        {
            Condition = string.IsNullOrWhiteSpace(text),
            Message = "Text is required"
        };

        private dynamic IsInvalid(DateTimeOffset date) => new
        {
            Condition = date == default,
            Message = "Date is required"
        };

        private static dynamic IsInvalid<T>(T value) => new
        {
            Condition = IsEnumInvalid(value),
            Message = "Value is not recognized"
        };

        private dynamic IsNotRecent(DateTimeOffset date) => new
        {
            Condition = IsDateNotRecent(date),
            Message = "Date is not recent"
        };

        private static bool IsEnumInvalid<T>(T value)
        {
            bool isDefined = Enum.IsDefined(typeof(T), value);

            return isDefined is false;
        }

        private static dynamic IsSame(
            DateTimeOffset firstDate,
            DateTimeOffset secondDate,
            string secondDateName) => new
            {
                Condition = firstDate == secondDate,
                Message = $"Date is the same as {secondDateName}"
            };

        private bool IsDateNotRecent(DateTimeOffset date)
        {
            DateTimeOffset currentDate = this.dateTimeBroker.GetCurrentDateTimeOffset();
            TimeSpan timeDifference = currentDate.Subtract(date);

            return timeDifference.TotalSeconds is > 60 or < 0;
        }

        private static void Validate(params (dynamic Rule, string Parameter)[] validations)
        {
            var invalidAttendanceException = new InvalidAttendanceException();

            foreach ((dynamic rule, string parameter) in validations)
            {
                if (rule.Condition)
                {
                    invalidAttendanceException.UpsertDataList(
                        key: parameter,
                        value: rule.Message);
                }
            }
            invalidAttendanceException.ThrowIfContainsErrors();
        }
    }
}
