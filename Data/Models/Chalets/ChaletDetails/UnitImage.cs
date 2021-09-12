using System;

namespace Data.Models.Chalets.ChaletDetails
{
    public class UnitImage : Entity
    {
        public bool IsPrimary { get; set; }
        public Guid FileId { get; set; }
        public Guid UnitId { get; set; }
        public File File { get; set; }
        public Unit Unit { get; set; }
    }
}