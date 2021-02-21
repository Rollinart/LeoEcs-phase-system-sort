using System;

namespace Rollin.LeoEcs
{
    [AttributeUsage(AttributeTargets.Class)]
    public class UpdateBeforeAttribute : Attribute
    {
        public Type Type { get; }
        
        public UpdateBeforeAttribute(Type type)
        {
            Type = type;
        }
    }
}