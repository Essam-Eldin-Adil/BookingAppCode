using System;
using System.Collections.Generic;
using System.Text;
using Data.Models.Chalets.ChaletDetails;

namespace Data.ViewModels
{
    public class UtilitiesViewModel
    {
        public UtilitiesViewModel()
        {
            Parameter = new Parameter();
            ParameterTranslation=new ParameterTranslation();
            ParameterGroupTranslation=new ParameterGroupTranslation();
            ParameterGroup = new ParameterGroup();
            ItemTree = new List<ItemTree>();
        }
        public ParameterTranslation ParameterTranslation { get; set; }
        public Parameter Parameter { get; set; }
        public ParameterGroupTranslation ParameterGroupTranslation { get; set; }
        public ParameterGroup ParameterGroup { get; set; }
        public List<ItemTree> ItemTree { get; set; }
    }

    public class ItemTree
    {
        public int Type { get; set; }
        public ParameterGroup ParameterGroup { get; set; }
        public List<ParameterGroup> ParameterGroups { get; set; }
        public bool HaveNodes { get; set; }
    }
}
