using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonAPI
{
    internal class CustomSaveTransaction : CustomTransaction
    {
        private string _path;
        private byte[] _data;
        public CustomSaveTransaction(MemoryStream stream, string filepath)
        {
            _data = stream.ToArray();
            _path = filepath;
        }

        public override void Process()
        {
            File.WriteAllBytes(_path, _data);
        }
    }
}
