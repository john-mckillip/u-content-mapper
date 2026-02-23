using UContentMapper.Core.Abstractions.Configuration;
using Umbraco.Cms.Core.Models;
using Umbraco.Extensions;

namespace UContentMapper.Umbraco15.Mapping
{
    public class MediaWithCropsToUrlConverter : ITypeConverter<MediaWithCrops, string>
    {
        private readonly Func<MediaWithCrops, string?> _urlResolver;

        public MediaWithCropsToUrlConverter(Func<MediaWithCrops, string?>? urlResolver = null)
        {
            _urlResolver = urlResolver ?? (source => source.Url());
        }

        public string Convert(MediaWithCrops source)
        {
            if (source is null)
            {
                return string.Empty;
            }

            try
            {
                return _urlResolver(source) ?? string.Empty;
            }
            catch (InvalidOperationException ex) when (ex.Message.Contains("IUrlProvider", StringComparison.Ordinal))
            {
                // In test or non-Umbraco environments, the URL provider may not be configured.
                // Fall back to empty string instead of throwing.
                return string.Empty;
            }
        }
    }
}