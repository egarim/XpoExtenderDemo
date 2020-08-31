using BIT.XpoExtender.Client;
using DevExpress.ExpressApp;
using System;

namespace BIT.XafExtender.Client
{
    public interface IProcess
    {
        IProcessResult Execute(IObjectSpace CurrentObjectSpace, XafApplication application, params object[] Args);
    }
}
