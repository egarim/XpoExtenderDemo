using BIT.XpoExtender.Client;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BIT.XafExtender.Client
{

    public static class Extensions
    {
        public static void RegisterXpoExtenderTypes(this ModuleBase Module)
        {
            Module.AdditionalExportedTypes.Add(typeof(BusinessObjectExtension));
            Module.AdditionalExportedTypes.Add(typeof(BusinessObjectField));
            Module.AdditionalExportedTypes.Add(typeof(CompiledModel));
            Module.AdditionalExportedTypes.Add(typeof(CompiledModelClass));
            Module.AdditionalExportedTypes.Add(typeof(CompiledModel));
            Module.AdditionalExportedTypes.Add(typeof(Process));
        }
        public static void AddXpoExtenderDefaultClassOptions(this ITypesInfo TypeInfo)
        {

            TypeInfo.FindTypeInfo(typeof(ExtendedModel)).AddAttribute(new DefaultClassOptionsAttribute());
            //TypeInfo.FindTypeInfo(typeof(Process)).AddAttribute(new DefaultClassOptionsAttribute());
            TypeInfo.FindTypeInfo(typeof(CompiledModel)).AddAttribute(new DefaultClassOptionsAttribute());
            TypeInfo.FindTypeInfo(typeof(CompiledModelClass)).AddAttribute(new DefaultClassOptionsAttribute());
            //TypeInfo.FindTypeInfo(typeof(BusinessObjectField)).AddAttribute(new DefaultClassOptionsAttribute());
            //TypeInfo.FindTypeInfo(typeof(BusinessObjectExtension)).AddAttribute(new DefaultClassOptionsAttribute());
        }
        public static void XafExtenderRegisterTypes(this ITypesInfo TypeInfo,IEnumerable<Type> types)
        {
            DevExpress.ExpressApp.DC.TypeInfo MyDynamicType;
            foreach (Type type in types)
            {
                MyDynamicType = (DevExpress.ExpressApp.DC.TypeInfo)TypeInfo.FindTypeInfo(type);
                MyDynamicType.RemoveAttributes<NonPersistentAttribute>();
                TypeInfo.RefreshInfo(type);
            }
        }
        public static void XafExtenderUpdateCurrentModel(this ITypesInfo TypeInfo, MetadataClient client)
        {
            client.UpdateSchema(TypeInfo);
        }
        public static void XafExtenderAddDefaultClassOptions(this ITypesInfo TypeInfo, Type type)
        {
            TypeInfo.FindTypeInfo(type)?.AddAttribute(new DefaultClassOptionsAttribute());
        }
    }
}
