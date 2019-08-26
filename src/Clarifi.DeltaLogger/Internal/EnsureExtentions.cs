using System;

namespace Clarifi.RoomMappingLogger.Internal
{
    internal static class EnsureExtentions
    {
        public static TypeEnsureExtensions Ensure(this Type type)
        {
            return new TypeEnsureExtensions(type);
        }


    }

    internal class TypeEnsureExtensions
    {
        public TypeEnsureExtensions(Type type)
        {
            Type = type;
        }

        public Type Type { get; }

        public TypeEnsureExtensions IsOfType(Type expected)
        {
            if (expected.IsAssignableFrom(Type) == false)
                throw new ArgumentException($"Type must be assignable to {expected.Name}.");
            return this;
        }

        public TypeEnsureExtensions IsOfType<T>() => IsOfType(typeof(T));

        public TypeEnsureExtensions HasAttribute<T>() where T : Attribute
        {
            if (Type.IsDefined(typeof(T), false) == false)
                throw new ArgumentException($"{Type.Name} Type must be attributed with {typeof(T).Name} attribute.");
            return this;
        }
    }

}
