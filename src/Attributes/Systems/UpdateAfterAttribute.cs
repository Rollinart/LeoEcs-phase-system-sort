using System;

namespace Rollin.LeoEcs
{
    /// <summary>
    /// Система с этим атрибутом будет вызываться ПОСЛЕ указанной в параметре
    /// Аккуратнее с зацикливанием!
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class UpdateAfterAttribute : Attribute
    {
        public Type Type { get; }

        /// <param name="type">ПОСЛЕ какой системы будет вызываться текущая</param>
        public UpdateAfterAttribute(Type type)
        {
            Type = type;
        }
    }
}