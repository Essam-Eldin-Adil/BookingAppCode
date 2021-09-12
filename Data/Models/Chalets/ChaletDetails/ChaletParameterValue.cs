using System;

namespace Data.Models.Chalets.ChaletDetails
{
    public class ChaletParameterValue : Entity
    {
        public Parameter Parameter { get; set; }
        public Guid ParameterId { get; set; }
        public Unit Unit { get; set; }
        public Guid UnitId { get; set; }
        public string Value { get; set; }
    }
}