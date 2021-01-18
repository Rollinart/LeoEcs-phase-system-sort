using System;

namespace Rollin.LeoEcs
{
    [AttributeUsage(AttributeTargets.Class)]
    public class SystemGroupAttribute : Attribute
    {
        public Enum GroupType { get; }

        public SystemGroupAttribute(object groupType)
        {
            GroupType = groupType as Enum;
        }
    }
}