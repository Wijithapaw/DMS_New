using DMS.Domain.Entities;
using DMS.Domain.Entities.Identity;
using DMS.Domain.Entities.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace DMS.Tests.Helpers
{
    public static class EntityHelper
    {
        public static Project CreateProject(int id, string title, string description, int ownerId, DateTime startDate, DateTime? endDate)
        {
            return new Project
            {
                Id = id,
                Title = title,
                Description = description,
                StartDate = startDate,
                EndDate = endDate,
                ProjectOwnerId = ownerId
            };
        }

        public static LookupHeader CreateLookupHeader(int id, string code, string description, bool deleted = false)
        {
            var header = new LookupHeader
            {
                Id = id,
                Code = code,
                Description = description,
                Deleted = deleted ? DateTime.UtcNow.Ticks : 0
            };

            return header;
        }

        public static Lookup CreateLookup(int id, int headerId, string code, string description, int sortOrder, bool deleted = false)
        {
            var lookup = new Lookup
            {
                Id = id,
                HeaderId = headerId,
                Code = code,
                Description = description,
                SortOrder = sortOrder,
                Deleted = deleted ? DateTime.UtcNow.Ticks : 0
            };

            return lookup;
        }

        public static User CreateUser(int id, string firstName, string lastName, string email, bool emailConfirmed)
        {
            var user = new User
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                EmailConfirmed = emailConfirmed,
                UserName = email,
                NormalizedEmail = email.ToUpper(),
                NormalizedUserName = email.ToUpper()
            };

            return user;
        }
    }
}
