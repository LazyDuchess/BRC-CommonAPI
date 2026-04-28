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
            var tempPath = _path + ".tmp";

            using(var fs = new FileStream(tempPath, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                fs.Write(_data, 0, _data.Length);
                fs.Flush(true);
            }

            if (File.Exists(_path))
            {
                File.Replace(tempPath, _path, null);
            }
            else
            {
                File.Move(tempPath, _path);
            }
        }
    }
}
