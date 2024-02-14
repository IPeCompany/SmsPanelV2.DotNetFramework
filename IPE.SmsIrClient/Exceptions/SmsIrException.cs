using System;
using System.Linq;

namespace IPE.SmsIrClient.Exceptions
{
    public class SmsIrException : Exception
    {
        internal readonly byte Status;

        internal SmsIrException(byte status, string message) : base(message)
        {
            Status = status;
        }

        public override string StackTrace
        {
            get
            {
                string[] lines = base.StackTrace.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                // Filter out lines containing your assembly name
                string[] filteredLines = lines
                                         .Where(line => !line.Contains("IPE.SmsIrClient"))
                                         .Skip(1)
                                         .ToArray();

                return string.Join(Environment.NewLine, filteredLines);
            }
        }
    }
}