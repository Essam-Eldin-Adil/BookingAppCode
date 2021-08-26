using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using static Domain.Enums;

namespace Domain
{
    public class Attributes
    {
        public static int GetEnum(HttpContext httpContext, Guid CommitteeUserTypeId)
        {
            var committeeUserRepository = (IRepository<Data.Models.CommitteeUserType>)httpContext.RequestServices.GetService(typeof(IRepository<Data.Models.CommitteeUserType>));

            var ChairmanId = committeeUserRepository.Table.Where(f => f.Type.Equals("Chairman")).SingleOrDefault().Id;
            var SecertaryId = committeeUserRepository.Table.Where(f => f.Type.Equals("Secretary")).SingleOrDefault().Id;
            var MemberId = committeeUserRepository.Table.Where(f => f.Type.Equals("Member")).SingleOrDefault().Id;
            var VisitorId = committeeUserRepository.Table.Where(f => f.Type.Equals("Visitor")).SingleOrDefault().Id;

            Dictionary<Guid, int> att = new Dictionary<Guid, int>()
                {
                   { ChairmanId,(int)CommitteeRoles.Chairman},
                   { SecertaryId,(int)CommitteeRoles.Secretary},
                   { MemberId,(int)CommitteeRoles.Member},
                   { VisitorId,(int)CommitteeRoles.Visitor}
                };
            return att[CommitteeUserTypeId];
        }

        public static Guid GetGuid(HttpContext httpContext, int EnumCommitteeUserTypeId)
        {
            var committeeUserRepository = (IRepository<Data.Models.CommitteeUserType>)httpContext.RequestServices.GetService(typeof(IRepository<Data.Models.CommitteeUserType>));

            var ChairmanId = committeeUserRepository.Table.Where(f => f.Type.Equals("Chairman")).SingleOrDefault().Id;
            var SecertaryId = committeeUserRepository.Table.Where(f => f.Type.Equals("Secretary")).SingleOrDefault().Id;
            var MemberId = committeeUserRepository.Table.Where(f => f.Type.Equals("Member")).SingleOrDefault().Id;
            var VisitorId = committeeUserRepository.Table.Where(f => f.Type.Equals("Visitor")).SingleOrDefault().Id;

            Dictionary<int, Guid> att = new Dictionary<int, Guid>()
                {
                   {(int)CommitteeRoles.Chairman, ChairmanId},
                   {(int)CommitteeRoles.Secretary, SecertaryId},
                   {(int)CommitteeRoles.Member, MemberId},
                   {(int)CommitteeRoles.Visitor, VisitorId}
                };
            return att[EnumCommitteeUserTypeId];
        }
    }
}
