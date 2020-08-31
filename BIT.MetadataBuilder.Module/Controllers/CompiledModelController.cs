//using BIT.MetadataBuilder.Client;
using BIT.XpoExtender.Client;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace BIT.MetadataBuilder.Module.Controllers
{

   
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class CompiledModelController : ViewController
    {
        public CompiledModelController()
        {
            InitializeComponent();
            this.TargetObjectType = typeof(CompiledModel);
            this.TargetViewType = ViewType.DetailView;
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void LoadAssembly_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            DevExpress.Xpo.Metadata.XPDictionary dict = new DevExpress.Xpo.Metadata.ReflectionDictionary();
            string assemblyFile = GetAssemblyPath();
            if (string.IsNullOrEmpty(assemblyFile))
                return;

            var asm = Assembly.LoadFrom(assemblyFile);

            var types = asm.GetExportedTypes();



            CompiledModel compiledModel = (CompiledModel)e.CurrentObject;
            compiledModel.Assembly = assemblyFile;
            compiledModel.Name = Path.GetFileName(assemblyFile);

            //System.Object,DevExpress.Xpo.IXPObject,DevExpress.Xpo.IXPSimpleObject,DevExpress.Xpo.Helpers.IXPClassInfoAndSessionProvider,DevExpress.Xpo.Helpers.IXPClassInfoProvider,DevExpress.Xpo.Metadata.Helpers.IXPDictionaryProvider,DevExpress.Xpo.Helpers.ISessionProvider,DevExpress.Xpo.Helpers.IObjectLayerProvider,DevExpress.Xpo.Helpers.IDataLayerProvider,DevExpress.Xpo.IXPCustomPropertyStore,DevExpress.Xpo.IXPModificationsStore,DevExpress.Xpo.IXPInvalidateableObject,DevExpress.Xpo.IXPReceiveOnChangedFromDelayedProperty,DevExpress.Xpo.IXPReceiveOnChangedFromArbitrarySource,System.ComponentModel.INotifyPropertyChanged,DevExpress.Xpo.Helpers.IXPImmutableHashCode,System.ComponentModel.IEditableObject,System.ComponentModel.ICustomTypeDescriptor,System.IComparable,DevExpress.Xpo.IXPReceiveOnChangedFromXPPropertyDescriptor,DevExpress.Xpo.XPObject,DevExpress.Xpo.XPCustomObject,DevExpress.Xpo.XPBaseObject,DevExpress.Xpo.PersistentBase,System.Object
            var AllXpoBases = "System.Object,DevExpress.Xpo.IXPObject,DevExpress.Xpo.IXPSimpleObject,DevExpress.Xpo.Helpers.IXPClassInfoAndSessionProvider,DevExpress.Xpo.Helpers.IXPClassInfoProvider,DevExpress.Xpo.Metadata.Helpers.IXPDictionaryProvider,DevExpress.Xpo.Helpers.ISessionProvider,DevExpress.Xpo.Helpers.IObjectLayerProvider,DevExpress.Xpo.Helpers.IDataLayerProvider,DevExpress.Xpo.IXPCustomPropertyStore,DevExpress.Xpo.IXPModificationsStore,DevExpress.Xpo.IXPInvalidateableObject,DevExpress.Xpo.IXPReceiveOnChangedFromDelayedProperty,DevExpress.Xpo.IXPReceiveOnChangedFromArbitrarySource,System.ComponentModel.INotifyPropertyChanged,DevExpress.Xpo.Helpers.IXPImmutableHashCode,System.ComponentModel.IEditableObject,System.ComponentModel.ICustomTypeDescriptor,System.IComparable,DevExpress.Xpo.IXPReceiveOnChangedFromXPPropertyDescriptor,DevExpress.Xpo.XPObject,DevExpress.Xpo.XPCustomObject,DevExpress.Xpo.XPBaseObject,DevExpress.Xpo.PersistentBase,System.Object";

            var AllXpoBasesList = AllXpoBases.Split(',').ToList();
            var GetTypes = asm.GetTypes();
            List<Type> AllTypes = new List<Type>();
            foreach (Type type in GetTypes)
            {
                var Parents = type.GetParentTypes();
                var matches = Parents.Where(bt => AllXpoBases.Contains(bt.FullName));
                int MatchCount = matches.Count();
                //All types should have at least 1 match that is the object base object
                Debug.WriteLine(MatchCount);
                if (MatchCount > 1)
                {
                    //AllTypes.Add(type);


                    var Found = compiledModel.CompiledModelClasses.FirstOrDefault(mc => mc.Name == type.Name);
                    if (ReferenceEquals(Found, null))
                    {
                        Found = this.ObjectSpace.CreateObject<CompiledModelClass>();
                        Found.Name = type.Name;
                        Found.CompiledModel = compiledModel;
                        Found.FullType = type.FullName;
                    }

                    if (this.View.ObjectSpace.IsModified)
                        this.View.ObjectSpace.CommitChanges();

                }
               
            }



            
        }

        protected virtual string GetAssemblyPath()
        {
            throw new NotImplementedException();
        }
    }
}
