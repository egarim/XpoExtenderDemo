using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;

using System.ComponentModel;

using DevExpress.Data.Filtering;

using System.Collections.Generic;

namespace BIT.XpoExtender.Client
{
   
    public class BusinessObjectField : XpoExtenderBaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public BusinessObjectField(Session session)
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
        CompiledModelClass compiledModelClass;
        TypeCode primitiveType;
        string name;


        public TypeCode PrimitiveType
        {
            get => primitiveType;
            set => SetPropertyValue(nameof(PrimitiveType), ref primitiveType, value);
        }
        //[DataSourceProperty("BusinessObjectExtension.Model.CompiledModel.CompiledModelClasses")]
        public CompiledModelClass CompiledModelClass
        {
            get => compiledModelClass;
            set => SetPropertyValue(nameof(CompiledModelClass), ref compiledModelClass, value);
        }


        BusinessObjectExtension businessObjectExtension;

        [Association("BusinessObjectExtension-BusinessObjectFields")]
        public BusinessObjectExtension BusinessObjectExtension
        {
            get => businessObjectExtension;
            set => SetPropertyValue(nameof(BusinessObjectExtension), ref businessObjectExtension, value);
        }
    }
}