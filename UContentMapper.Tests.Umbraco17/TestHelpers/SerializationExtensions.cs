using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
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

            Execute.Assertion
                .BecauseOf(because, becauseArgs)
                .Given(() =>
                {
                    try
                    {
                        string json = JsonSerializer.Serialize(subject, options);
                        var deserializedObject = JsonSerializer.Deserialize<T>(json, options);
                        return deserializedObject != null;
                    }
                    catch (Exception ex)
                    {
                        throw new AssertionFailedException($"Expected {typeof(T).Name} to be JSON serializable, but serialization failed with: \"{ex.Message}\".");
                    }
                })
                .ForCondition(result => result)
                .FailWith("Expected {0} to be JSON serializable, but it was not.", subject);

            return new AndConstraint<ObjectAssertions>(assertions);
        }
    }
}
