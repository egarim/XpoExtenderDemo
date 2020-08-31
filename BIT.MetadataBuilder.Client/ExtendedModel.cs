using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;

using System.ComponentModel;

using DevExpress.Data.Filtering;

using System.Collections.Generic;


namespace BIT.XpoExtender.Client
{
    public class ExtendedModel : XpoExtenderBaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public ExtendedModel(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        CompiledModel compiledModel;
        string name;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name
        {
            get => name;
            set => SetPropertyValue(nameof(Name), ref name, value);
        }
        [Association("Model-CompiledModels")]
        public XPCollection<CompiledModel> CompiledModels
        {
            get
            {
                return GetCollection<CompiledModel>(nameof(CompiledModels));
            }
        }
        //public CompiledModel CompiledModel
        //{
        //    get => compiledModel;
        //    set => SetPropertyValue(nameof(CompiledModel), ref compiledModel, value);
        //}
        [Association("Model-BusinessObjectExtensions"), DevExpress.Xpo.Aggregated()]
        public XPCollection<BusinessObjectExtension> BusinessObjectExtensions
        {
            get
            {
                return GetCollection<BusinessObjectExtension>(nameof(BusinessObjectExtensions));
            }
        }


    }
}