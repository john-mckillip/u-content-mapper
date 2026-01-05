using UContentMapper.Core.Abstractions.Configuration;
using Umbraco.Cms.Core.Models;
using Umbraco.Extensions;

namespace UContentMapper.Umbraco15.Mapping
{
    public class MediaWithCropsToUrlConverter : ITypeConverter<MediaWithCrops, string>
    {
        public string Convert(MediaWithCrops source)
        {
            if (source is null)
            {
                return string.Empty;
            }

            try
            {
                return source.Url() ?? string.Empty;
            }
            catch
            {
                // In test or non-Umbraco environments, the URL provider may not be configured.
                // Fall back to empty string instead of throwing.
                return string.Empty;
            }
        }
    }
}