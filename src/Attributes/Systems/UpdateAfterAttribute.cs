using System;

namespace Rollin.LeoEcs
{
    [AttributeUsage(AttributeTargets.Class)]
    public class UpdateAfterAttribute : Attribute
    {
        public Type Type { get; }
        
        public UpdateAfterAttribute(Type type)
        {
            Type = type;
        }
    }
}