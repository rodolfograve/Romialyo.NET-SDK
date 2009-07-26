using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Romialyo.Web
{
    public class FileDTO
    {
        public FileDTO(string fileName, Stream dataStream)
        {
            FileName = fileName;
            DataStream = dataStream;
        }

        public readonly string FileName;

        protected readonly Stream DataStream;
        public Stream GetDataStream()
        {
            return DataStream;
        }

    }
}
