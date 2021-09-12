using System;

namespace Data.Models.Chalets
{
    public class ChaletImage:Entity
    {
        public bool IsPrimary { get; set; }
        public Guid FileId { get; set; }
        public Guid ChaletId { get; set; }
        public File File { get; set; }
        public Chalet Chalet { get; set; }
    }
}
