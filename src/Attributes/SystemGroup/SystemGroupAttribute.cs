using System;

namespace Rollin.LeoEcs
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SystemGroupAttribute : Attribute
    {
        public Enum GroupType { get; }
        public int GroupIndex { get; }

        public SystemGroupAttribute(object groupType)
        {
            GroupIndex = (int) groupType;
            GroupType = groupType as Enum;
        }
    }
}