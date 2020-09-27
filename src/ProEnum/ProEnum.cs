using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace ProEnum
{
    [Serializable]
    [DebuggerDisplay("{Name} - {Value}")]
    public abstract class ProEnum : IComparable
    {
#pragma warning disable 8618
        /// <summary>
        ///     Default constructor, declared for Persistence, Serialization etc.,
        /// </summary>
        protected ProEnum() { }
#pragma warning restore 8618

        protected ProEnum(int value, string name)
        {
            Value = value;
            Name = name;
        }

        public string Name { get; }

        public int Value { get; }

        public int CompareTo(object other) => Value.CompareTo(((ProEnum)other).Value);


        public override string ToString() => Name;

        public static IEnumerable<TEnum> GetAll<TEnum>() where TEnum : ProEnum
        {
            var fields = typeof(TEnum).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            return fields.Select(f => f.GetValue(null)).Cast<TEnum>();
        }

        public override bool Equals(object obj)
        {
            if(!(obj is ProEnum otherValue))
                return false;

            var typeMatches = GetType() == obj.GetType();
            var valueMatches = Value.Equals(otherValue.Value);

            return typeMatches && valueMatches;
        }

        public override int GetHashCode() => Value.GetHashCode();

        public static int AbsoluteDifference(ProEnum firstValue, ProEnum secondValue)
        {
            var absoluteDifference = Math.Abs(firstValue.Value - secondValue.Value);
            return absoluteDifference;
        }

        public static bool TryGetFromValueOrName<TEnum>(string valueOrName, out TEnum? enumeration) where TEnum : ProEnum =>
            TryParse(item => item.Name == valueOrName, out enumeration) ||
            (int.TryParse(valueOrName, out var value) && TryParse(item => item.Value == value, out enumeration));

        public static TEnum FromValue<TEnum>(int value) where TEnum : ProEnum
        {
            var matchingItem = Parse<TEnum, int>(value, "nameOrValue", item => item.Value == value);
            return matchingItem;
        }

        public static TEnum FromName<TEnum>(string name) where TEnum : ProEnum
        {
            var matchingItem = Parse<TEnum, string>(name, "name", item => item.Name == name);
            return matchingItem;
        }

        private static bool TryParse<TEnum>(Func<TEnum, bool> predicate, out TEnum? enumeration) where TEnum : ProEnum
        {
            enumeration = GetAll<TEnum>().FirstOrDefault(predicate);
            return enumeration != null;
        }

        private static TEnum Parse<TEnum, TIntOrString>(TIntOrString nameOrValue, string description, Func<TEnum, bool> predicate) where TEnum : ProEnum
        {
            var matchingItem = GetAll<TEnum>().FirstOrDefault(predicate);

            if(matchingItem == null)
                throw new InvalidOperationException($"'{nameOrValue}' is not a valid {description} in {typeof(TEnum)}");

            return matchingItem;
        }
    }
}