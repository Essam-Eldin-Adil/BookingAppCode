using Data.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Domain
{
    public static partial class Enums
    {
        public enum Hour
        {
            h24,
            h12
        }

        public enum UserType
        {
            Internal,
            External,
            Former
        }

        public enum CommitteeUserType
        {
            Chairman,
            Secretary,
            Member,
            Visitor,
        }
        public enum MeetingStatus
        {
            Draft,
            Scheduled,
            Open,
            Closed
        }
        public enum MeetingUserStatus
        {
            WaitingForReply,
            Accept,
            Apologize,
            Delegate
        }

        public enum TopicTransferStatus
        {
            Pending,
            Accept,
            Reject
        }


        public enum CommitteeRoles
        {
            Chairman,
            Secretary,
            Member,
            Visitor,
        }
    }
}
