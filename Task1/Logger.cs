using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Task1
{
    /// <summary>
    /// Provides logging into file
    /// </summary>
    public class Logger
    {
        private readonly string _fileName;

        /// <summary>
        /// Initializes new Logger instance with the name of file where logging will be made into
        /// </summary>
        /// <param name="filename">Logging file name</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public Logger(string filename)
        {
            if (filename == null)
                throw new ArgumentNullException();

            if (filename.Equals(string.Empty))
                throw new ArgumentException();

            _fileName = filename;
            if (!File.Exists(_fileName))
                File.Create(_fileName);
        }

        /// <summary>
        /// Writes log message into file
        /// </summary>
        /// <param name="message">Log message</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public void WriteLog(string message)
        {
            if (message == null)
                throw new ArgumentNullException();

            if (message.Equals(string.Empty))
                throw new ArgumentException();

            try
            {
                using (var streamWriter = new StreamWriter(_fileName, true))
                {
                    streamWriter.WriteLine(message);
                }
            }
            catch (Exception e)
            {
                throw new Exception(@"Error while writing log");
            }
        }
    }
}
