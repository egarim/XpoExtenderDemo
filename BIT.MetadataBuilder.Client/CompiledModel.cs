using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;

using System.ComponentModel;

using DevExpress.Data.Filtering;

using System.Collections.Generic;


namespace BIT.XpoExtender.Client
{
 

    [DefaultProperty("Name")]
    public class CompiledModel : XpoExtenderBaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public CompiledModel(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        
        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name
        {
            get => name;
            set => SetPropertyValue(nameof(Name), ref name, value);
        }
        string name;
        string assembly;
        [Size(SizeAttribute.Unlimited)]
        public string Assembly
        {
            get => assembly;
            set => SetPropertyValue(nameof(Assembly), ref assembly, value);
        }
        [Association("CompiledModel-CompiledModelClasses"), DevExpress.Xpo.Aggregated()]
        public XPCollection<CompiledModelClass> CompiledModelClasses
        {
            get
            {
                return GetCollection<CompiledModelClass>(nameof(CompiledModelClasses));
            }
        }
        [Association("Model-CompiledModels")]
        public XPCollection<ExtendedModel> Models
        {
            get
            {
                return GetCollection<ExtendedModel>(nameof(Models));
            }
        }

    }
}