using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;

using System.ComponentModel;

using DevExpress.Data.Filtering;

using System.Collections.Generic;


namespace BIT.XpoExtender.Client
{
    
    public class BusinessObjectExtension : XpoExtenderBaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public BusinessObjectExtension(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        CompiledModelClass compiledModelClass;
        ExtendedModel model;
        string type;
        string name;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name
        {
            get => name;
            set => SetPropertyValue(nameof(Name), ref name, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Type
        {
            get => type;
            set => SetPropertyValue(nameof(Type), ref type, value);
        }

        [Association("Model-BusinessObjectExtensions")]
        public ExtendedModel Model
        {
            get => model;
            set => SetPropertyValue(nameof(Model), ref model, value);
        }

        //[DataSourceProperty("Model.CompiledModel.CompiledModelClasses")]
        public CompiledModelClass CompiledModelClass
        {
            get => compiledModelClass;
            set => SetPropertyValue(nameof(CompiledModelClass), ref compiledModelClass, value);
        }
        [Association("BusinessObjectExtension-BusinessObjectFields"),DevExpress.Xpo.Aggregated()]
        public XPCollection<BusinessObjectField> BusinessObjectFields
        {
            get
            {
                return GetCollection<BusinessObjectField>(nameof(BusinessObjectFields));
            }
        }
        protected override void OnChanged(string propertyName, object oldValue, object newValue)
        {
            base.OnChanged(propertyName, oldValue, newValue);
            if(propertyName==nameof(CompiledModelClass))
            {
                this.Type = CompiledModelClass?.FullType;
            }
        }
    }
}