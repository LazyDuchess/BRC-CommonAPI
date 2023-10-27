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
        public CustomSaveTransaction(byte[] data, string filepath)
        {
            _data = data;
            _path = filepath;
        }

        public override void Process()
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_path));
            File.WriteAllBytes(_path, _data);
        }
    }
}
