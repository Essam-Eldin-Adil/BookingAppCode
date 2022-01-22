using System;

namespace Data.Models.Chalets.ChaletDetails
{
    public class ResortParameterValue : Entity
    {
        public Parameter Parameter { get; set; }
        public Guid ParameterId { get; set; }
        public Chalet Chalet { get; set; }
        public Guid ChaletId { get; set; }
        public string Value { get; set; }
    }
}