using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romialyo.Web
{
    public class WebRequestDTO
    {

        public WebRequestDTO(IEnumerable<KeyValuePair<string, string>> parameters, IEnumerable<KeyValuePair<string, FileDTO>> files)
        {
            Parameters = parameters;
            Files = files;
        }

        public readonly IEnumerable<KeyValuePair<string, string>> Parameters;

        public readonly IEnumerable<KeyValuePair<string, FileDTO>> Files;

    }
}
