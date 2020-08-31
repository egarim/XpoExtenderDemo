using System;

namespace BIT.XpoExtender.Client
{
    public interface IProcessResult
    {
        Exception Exception { get; set; }
        bool Success { get; set; }


    }
}
