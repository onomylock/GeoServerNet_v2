using System.Text;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using Shared.Common.Exceptions;

namespace Shared.Common.Helpers;

public static class MultipartSectionHelper
{
    public static Encoding GetEncoding(this MultipartSection section)
    {
        var hasMediaTypeHeader = MediaTypeHeaderValue.TryParse(section.ContentType, out var mediaType);

        // UTF-7 is insecure and shouldn't be honored. UTF-8 succeeds in 
        // most cases.
#pragma warning disable SYSLIB0001
        if (!hasMediaTypeHeader || Encoding.UTF7.Equals(mediaType.Encoding))
#pragma warning restore SYSLIB0001
            return Encoding.UTF8;

        return mediaType.Encoding;
    }

    public class MultipartSectionNotFoundException : LocalizedException;

    public class MultipartSectionContentDispositionParseFailedException : LocalizedException;

    public class MultipartSectionContentDispositionFileExpectedException : LocalizedException;

    public class MultipartSectionContentDispositionFormExpectedException : LocalizedException;

    public class MultipartSectionEncodingRetrievalFailedException : LocalizedException;
}