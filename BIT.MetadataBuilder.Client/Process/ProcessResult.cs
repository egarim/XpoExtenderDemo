using System;

namespace BIT.XpoExtender.Client
{
    public class ProcessResult : IProcessResult
    {
        public Exception Exception { get; set; }
        public bool Success { get; set; }
    }
}
