using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CommonAPI
{
    /// <summary>
    /// Writes data in a multithreaded fashion.
    /// </summary>
    public class CustomStorage
    {
        internal static CustomStorage Instance;
        private Thread _storageThread;
        private bool _running = true;
        private Queue<CustomTransaction> _transactionQueue = new Queue<CustomTransaction>();

        internal CustomStorage()
        {
            Instance = this;
            _storageThread = new Thread(new ThreadStart(StorageLoop));
            _storageThread.IsBackground = true;
            _storageThread.Name = "CustomStorageThread";
            _storageThread.Start();
        }

        private void StorageLoop()
        {
            while(_running)
            {
                if (_transactionQueue.Count > 0)
                {
                    var transaction = _transactionQueue.Peek();
                    transaction.Process();
                    _transactionQueue.Dequeue();
                }
                else
                    Thread.Sleep(100);
            }
        }

        internal void HandleQuit()
        {
            CommonAPIPlugin.Log.LogInfo("Flushing custom save files...");
            var timeLimit = DateTime.Now + TimeSpan.FromSeconds(15D);
            while(_transactionQueue.Count > 0)
            {
                if (DateTime.Now > timeLimit)
                {
                    throw new Exception("Could not finish saving to save files, the thread was aborted prematurely!");
                }
            }
            _running = false;
        }

        /// <summary>
        /// Enqueues a file write.
        /// </summary>
        public void WriteFile(byte[] data, string path)
        {
            var transaction = new CustomSaveTransaction(data, path);
            _transactionQueue.Enqueue(transaction);
        }
    }
}
