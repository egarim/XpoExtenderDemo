
using DevExpress.Xpo;
using System;
using System.Linq;

namespace BIT.XpoExtender.Client
{
 
    public class Process : XpoExtenderBaseObject
    {
        public Process(Session session) : base(session)
        { }
        
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string TypeName
        {
            get => typeName;
            set => SetPropertyValue(nameof(TypeName), ref typeName, value);
        }
        
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string AssemblyName
        {
            get => assemblyName;
            set => SetPropertyValue(nameof(AssemblyName), ref assemblyName, value);
        }

        public ProcessType ProcessType
        {
            get => processType;
            set => SetPropertyValue(nameof(ProcessType), ref processType, value);
        }
        string assemblyName;
        ProcessType processType;
        string typeName;
        int version;
        PersistentFile files;
        string name;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name
        {
            get => name;
            set => SetPropertyValue(nameof(Name), ref name, value);
        }

        public PersistentFile Files
        {
            get => files;
            set => SetPropertyValue(nameof(Files), ref files, value);
        }

        public int Version
        {
            get => version;
            set => SetPropertyValue(nameof(Version), ref version, value);
        }
    }
}