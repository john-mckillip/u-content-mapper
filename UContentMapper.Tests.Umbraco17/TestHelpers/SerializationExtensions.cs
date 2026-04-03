using FluentAssertions;
using FluentAssertions.Primitives;
using NUnit.Framework;
using System.Text.Json;

namespace UContentMapper.Tests.Umbraco17.TestHelpers
{
    /// <summary>
    /// Extension methods for serialization testing
    /// </summary>
    public static class SerializationExtensions
    {
        /// <summary>
        /// Verifies that an object can be serialized and deserialized using JSON
        /// </summary>
        public static AndConstraint<ObjectAssertions> BeJsonSerializable<T>(
            this ObjectAssertions assertions,
            string because = "",
            params object[] becauseArgs) where T : class
        {
            var subject = assertions.Subject as T;

            if (subject == null)
            {
                throw new ArgumentException("Subject must not be null and must be of type " + typeof(T).Name);
            }

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                IncludeFields = true
            };

            try
            {
                string json = JsonSerializer.Serialize(subject, options);
                var deserializedObject = JsonSerializer.Deserialize<T>(json, options);
                deserializedObject.Should().NotBeNull(because, becauseArgs);
            }
            catch (Exception ex)
            {
                throw new AssertionException($"Expected {typeof(T).Name} to be JSON serializable, but serialization failed with: \"{ex.Message}\".");
            }

            return new AndConstraint<ObjectAssertions>(assertions);
        }
    }
}
