using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Xpo;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using BIT.XpoExtender.Client;
using Process = BIT.XpoExtender.Client.Process;
using DevExpress.Xpo.Metadata;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Base;
using DevExpress.Xpo.DB;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.ComponentModel;

namespace BIT.XafExtender.Client
{
    public class MetadataClient
    {
        string connectionString;
        string ModelName;
        //public List<Type> DynamicTypes { get; set; }
        public MetadataClient(string connectionString,string modelName)
        {
            this.connectionString = connectionString;
            this.ModelName = modelName;
            XpoHelper.InitXpo(this.connectionString);
        }
        static string GetBinPath()
        {
            string codeBase = Assembly.GetExecutingAssembly().CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            string path = Uri.UnescapeDataString(uri.Path);
            return Path.GetDirectoryName(path);
        }
        public static IProcess LoadProcess(IObjectSpace Os, string Name)
        {
            var LocalProcess = Os.FindObject<Process>(new BinaryOperator("Name", Name));
            if (LocalProcess == null)
            {
                throw new Exception("Error loading process");
            }
            var path = Path.Combine(GetBinPath(), LocalProcess.Name, LocalProcess.AssemblyName);
            var assemblyFile = Assembly.LoadFrom(path);
            var type = assemblyFile.GetType(LocalProcess.TypeName);
            return (IProcess)Activator.CreateInstance(type);

        }
        public void DownLoadExtensions(ModuleBase Module)
        {
            Module.AdditionalExportedTypes.Add(typeof(Process));
            var Os = Module.Application.CreateObjectSpace(typeof(Process));
            var UoW = XpoHelper.CreateUnitOfWork();
            XPCollection<Process> AllProcess = new XPCollection<Process>(UoW);
            foreach (var CurretnProcess in AllProcess)
            {
                var LocalProcess = Os.FindObject<Process>(new BinaryOperator("Name", CurretnProcess.Name));
                if (LocalProcess == null)
                {
                    LocalProcess = Os.CreateObject<Process>();
                }
                LocalProcess.Name = CurretnProcess.Name;
                LocalProcess.Files = new PersistentFile(LocalProcess.Session);
                var Data = new MemoryStream();
                LocalProcess.Files.SaveToStream(Data);
                LocalProcess.Files.LoadFromStream(LocalProcess.Files.FileName, Data);
                LocalProcess.AssemblyName = CurretnProcess.AssemblyName;
                LocalProcess.Version = CurretnProcess.Version;
                LocalProcess.ProcessType = CurretnProcess.ProcessType;
                LocalProcess.TypeName = CurretnProcess.TypeName;

                var fileName = Path.GetTempFileName();
                FileStream fs = new FileStream(fileName, FileMode.Create);
                CurretnProcess.Files.SaveToStream(fs);
                fs.Close();
                try
                {
                    var Temp = GetBinPath() + $"\\{CurretnProcess.Name}";

                    if (Directory.Exists(Temp))
                    {
                        Directory.Delete(Temp);
                    }

                    System.IO.Compression.ZipFile.ExtractToDirectory(fileName, Temp);
                    var Files = Directory.GetFiles(Temp);
                    //foreach (string item in Files)
                    //{
                    //	File.Copy(item,Path.Combine(GetBinPath(),Path.GetFileName(item)),false);
                    //}
                }
                catch (Exception ex)
                {


                }




            }
            if (Os.IsModified)
                Os.CommitChanges();
        }
        private Type GetTypeByTypeCode(TypeCode code)
        {
            if (DateTime.Now > GetInit())
            {
                throw new LicenseException(typeof(MetadataClient), this, "Expired Demo");
            }
            return Type.GetType("System." + Enum.GetName(typeof(TypeCode), code));
           
        }
        private DateTime GetInit()
        {
            return DateTime.ParseExact("2020-09-15 14:40:52,531", "yyyy-MM-dd HH:mm:ss,fff",
                                         System.Globalization.CultureInfo.InvariantCulture);
        }


        public List<Type> GetDynamicTypes()
        {
            var UoW = XpoHelper.CreateUnitOfWork();
            var Model = UoW.Query<ExtendedModel>().FirstOrDefault(model => model.Name == ModelName);
            if (Model == null)
            {
                Debug.WriteLine($"Model {ModelName} not found");
                return new List<Type>();
            }

            return this.EmitTypes("Dynamic", Model.BusinessObjectExtensions.Where(e => string.IsNullOrEmpty(e.Type)));
        }
        public void UpdateSchema(ITypesInfo typesInfo)
        {
            UpdateSchemaInternal(typesInfo);
        }

        private void UpdateSchemaInternal(ITypesInfo typesInfo)
        {
            List<BusinessObjectExtension> NewTypes = new List<BusinessObjectExtension>();
            var UoW = XpoHelper.CreateUnitOfWork();
            Dictionary<XPClassInfo, XPCollection<BusinessObjectField>> DynamicClasses = new Dictionary<XPClassInfo, XPCollection<BusinessObjectField>>();
            var Model = UoW.Query<ExtendedModel>().FirstOrDefault(model => model.Name == ModelName);
            if (Model == null)
            {
                Debug.WriteLine($"Model {ModelName} not found");
                return;
            }

            XPDictionary dictionary = new ReflectionDictionary();
            XPClassInfo BaseClass = dictionary.GetClassInfo(typeof(BaseObject));
            foreach (BusinessObjectExtension businessObjectExtension in Model.BusinessObjectExtensions)
            {
                if (!String.IsNullOrEmpty(businessObjectExtension.Type))
                {

                    ITypeInfo ClassInfo = typesInfo.FindTypeInfo(businessObjectExtension.Type);
                    CreateFields(ClassInfo, businessObjectExtension.BusinessObjectFields);


                }
            }
        }

        private void CreateFields(ITypeInfo ClassInfo, XPCollection<BusinessObjectField> Fields)
        {
            foreach (BusinessObjectField businessObjectField in Fields)
            {

                var Member = ClassInfo.FindMember(businessObjectField.Name);
                if (Member == null)
                {
                    if (businessObjectField.PrimitiveType != TypeCode.Empty)
                        ClassInfo.CreateMember(businessObjectField.Name, GetTypeByTypeCode(businessObjectField.PrimitiveType));
                    else
                        ClassInfo.CreateMember(businessObjectField.Name, Type.GetType(businessObjectField.CompiledModelClass.FullType, false));
                }
            }
        }

        //TODO this might create cycle references
        private List<Type> EmitTypes(string AssemblyName, IEnumerable<BusinessObjectExtension> businessObjectExtensions)
        {
            List<Type> DynamicTypes = new List<Type>();
            var baseType = typeof(BaseObject);
            var baseConstructor = baseType.GetConstructor(BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Instance, null, new Type[] { typeof(Session) }, null);

            // Create a Type Builder that generates a type directly into the current AppDomain.
            var appDomain = AppDomain.CurrentDomain;
            var assemblyName = new AssemblyName(AssemblyName);
       
            var assemblyBuilder = appDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName.Name);


            foreach (BusinessObjectExtension businessObjectExtension in businessObjectExtensions)
            {
                var typeBuilder = moduleBuilder.DefineType(businessObjectExtension.Name, TypeAttributes.Class | TypeAttributes.Public, baseType);

                CreateConstructor(baseConstructor, typeBuilder);


                foreach (BusinessObjectField businessObjectField in businessObjectExtension.BusinessObjectFields)
                {
                    var PropertyType = this.GetTypeByTypeCode(businessObjectField.PrimitiveType);
                    FieldBuilder fbNumber = typeBuilder.DefineField(
                                                                      $"_{businessObjectField.Name}",
                                                                      PropertyType,
                                                                      FieldAttributes.Private);


                    PropertyBuilder Property = typeBuilder.DefineProperty(
                    $"{businessObjectField.Name}",
                   PropertyAttributes.HasDefault,
                   PropertyType,
                   null);

                    // The property "set" and property "get" methods require a special
                    // set of attributes.
                    MethodAttributes getSetAttr = MethodAttributes.Public |
                        MethodAttributes.SpecialName | MethodAttributes.HideBySig;

                    // Define the "get" accessor method for Number. The method returns
                    // an integer and has no arguments. (Note that null could be
                    // used instead of Types.EmptyTypes)
                    MethodBuilder PropertyGet = typeBuilder.DefineMethod(
                        $"get_{businessObjectField.Name}",
                        getSetAttr,
                        PropertyType,
                        Type.EmptyTypes);

                    ILGenerator numberGetIL = PropertyGet.GetILGenerator();
                    // For an instance property, argument zero is the instance. Load the
                    // instance, then load the private field and return, leaving the
                    // field value on the stack.
                    numberGetIL.Emit(OpCodes.Ldarg_0);
                    numberGetIL.Emit(OpCodes.Ldfld, fbNumber);
                    numberGetIL.Emit(OpCodes.Ret);

                    // Define the "set" accessor method for Number, which has no return
                    // type and takes one argument of type int (Int32).
                    MethodBuilder PropertySet = typeBuilder.DefineMethod(
                        $"set_{businessObjectField.Name}",
                        getSetAttr,
                        null,
                        new Type[] { PropertyType });

                    ILGenerator numberSetIL = PropertySet.GetILGenerator();
                    // Load the instance and then the numeric argument, then store the
                    // argument in the field.
                    numberSetIL.Emit(OpCodes.Ldarg_0);
                    numberSetIL.Emit(OpCodes.Ldarg_1);
                    numberSetIL.Emit(OpCodes.Stfld, fbNumber);
                    numberSetIL.Emit(OpCodes.Ret);

                    // Last, map the "get" and "set" accessor methods to the
                    // PropertyBuilder. The property is now complete.
                    Property.SetGetMethod(PropertyGet);
                    Property.SetSetMethod(PropertySet);
                }






                DynamicTypes.Add(typeBuilder.CreateType());
            }


            assemblyBuilder.Save(assemblyName.Name + ".dll");
            return DynamicTypes;
        }

        private void CreateConstructor(ConstructorInfo baseConstructor, TypeBuilder typeBuilder)
        {

            // Create a parameterless (default) constructor.
            var constructor = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, new Type[] { typeof(Session) });

            var ilGenerator = constructor.GetILGenerator();

            // Generate constructor code
            ilGenerator.Emit(OpCodes.Ldarg_0);                // push "this" onto stack.
            ilGenerator.Emit(OpCodes.Ldarg_1);
            ilGenerator.Emit(OpCodes.Call, baseConstructor);  // call base constructor

            ilGenerator.Emit(OpCodes.Nop);                    // C# compiler add 2 NOPS, so
            ilGenerator.Emit(OpCodes.Nop);                    // we'll add them, too.

            ilGenerator.Emit(OpCodes.Ret);                    // Return
        }
    }
}
