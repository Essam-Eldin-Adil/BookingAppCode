using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Domain
{
    public static partial class Enums
    {
        public enum PropertyType
        {
            All,
            Resort,
            Break,
            Villa
        }
        public enum Hour
        {
            h24,
            h12
        }

        public enum UserType
        {
            Admin=1,
            BookAdmin,
            BookUser,
            EndUser
        }

        public enum ParameterType
        {
            Text,
            Checkbox
        }

        public enum Direction
        {
            North=1,
            South,
            East,
            West,
            Northeast,
            Southeast,
            NorthWest,
            Southwest
        }

        public enum Status
        {
            New,
            Confirmed,
            Cancled
        }
    }
}
