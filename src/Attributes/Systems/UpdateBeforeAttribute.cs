using System;

namespace Rollin.LeoEcs
{
    /// <summary>
    /// Система с этим атрибутом будет вызываться ДО указанной в параметре
    /// Аккуратнее с зацикливанием!
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class UpdateBeforeAttribute : Attribute
    {
        public Type Type { get; }

        /// <param name="type">ДО какой системы будет вызываться текущая</param>
        public UpdateBeforeAttribute(Type type)
        {
            Type = type;
        }
    }
}