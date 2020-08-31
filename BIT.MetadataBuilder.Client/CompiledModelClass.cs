using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;

using System.ComponentModel;

using DevExpress.Data.Filtering;

using System.Collections.Generic;


namespace BIT.XpoExtender.Client 
{ 

    public class CompiledModelClass : XpoExtenderBaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public CompiledModelClass(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        CompiledModel compiledModel;
        string fullType;
        string name;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name
        {
            get => name;
            set => SetPropertyValue(nameof(Name), ref name, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string FullType
        {
            get => fullType;
            set => SetPropertyValue(nameof(FullType), ref fullType, value);
        }

        [Association("CompiledModel-CompiledModelClasses")]
        public CompiledModel CompiledModel
        {
            get => compiledModel;
            set => SetPropertyValue(nameof(CompiledModel), ref compiledModel, value);
        }
    }
}