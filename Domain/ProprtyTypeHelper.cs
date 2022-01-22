using System;
using System.Collections.Generic;
using System.Resources;
using System.Text;
using Resources;

namespace Domain
{
    public static class PropertyTypeHelper
    {
        public static string GetResource(int propertyType,string key)
        {
            switch (propertyType)
            {
                case (int)Enums.PropertyType.Resort:
                    key += (int)Enums.PropertyType.Resort;
                    break;
                case (int)Enums.PropertyType.Reset:
                    key += (int)Enums.PropertyType.Reset; 
                    break;
                case (int)Enums.PropertyType.Villa:
                    key += (int)Enums.PropertyType.Villa;
                    break;
            }
            return Resource.ResourceManager.GetString(key);
        }
    }
}
