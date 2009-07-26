using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Romialyo.Web
{
    public class WebRequestDTOBuilder
    {

        protected WebRequestDTOBuilder()
        {
            Parameters = new List<KeyValuePair<string, string>>();
            Files = new List<KeyValuePair<string, FileDTO>>();
        }

        public static WebRequestDTOBuilder New()
        {
            return new WebRequestDTOBuilder();
        }

        public WebRequestDTOBuilder WithParameter(string key, string value)
        {
            Parameters.Add(new KeyValuePair<string, string>(key, value));
            return this;
        }

        public WebRequestDTOBuilder WithFile(string key, string fileName, Stream dataStream)
        {
            Files.Add(new KeyValuePair<string, FileDTO>(key, new FileDTO(fileName, dataStream)));
            return this;
        }

        protected IList<KeyValuePair<string, string>> Parameters;

        protected IList<KeyValuePair<string, FileDTO>> Files;

        public static implicit operator WebRequestDTO(WebRequestDTOBuilder builder)
        {
            return new WebRequestDTO(builder.Parameters, builder.Files);
        }

    }
}
