using System;
using System.Text;
using System.Linq;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using System.Collections.Generic;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Model.DomainLogics;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using SampleApp.Module.BusinessObjects;
using BIT.XafExtender.Client;
using BIT.XpoExtender.Client;

namespace SampleApp.Module
{
    // For more typical usage scenarios, be sure to check out https://docs.devexpress.com/eXpressAppFramework/DevExpress.ExpressApp.ModuleBase.
    public sealed partial class SampleAppModule : ModuleBase
    {
        MetadataClient MetadataClient;
        List<Type> types;
        public SampleAppModule()
        {
            InitializeComponent();


            BaseObject.OidInitializationMode = OidInitializationMode.AfterConstruction;

            string ModelName = System.Configuration.ConfigurationManager.AppSettings["ExtendedModelName"];
            MetadataClient = new MetadataClient(@"Integrated Security=SSPI;Pooling=false;Data Source=(localdb)\mssqllocaldb;Initial Catalog=XpoExtender", ModelName);
            types = this.MetadataClient.GetDynamicTypes();
            this.AdditionalExportedTypes.AddRange(types);

            //TODO add process example
            //this.AdditionalExportedTypes.Add(typeof(Process));
        }
        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB)
        {
            ModuleUpdater updater = new DatabaseUpdate.Updater(objectSpace, versionFromDB);
            return new ModuleUpdater[] { updater };
        }
        public override void Setup(XafApplication application)
        {
            base.Setup(application);
            // Manage various aspects of the application UI and behavior at the module level.
        }
        public override void CustomizeTypesInfo(ITypesInfo typesInfo)
        {
            base.CustomizeTypesInfo(typesInfo);
            CalculatedPersistentAliasHelper.CustomizeTypesInfo(typesInfo);
            

         
            typesInfo.XafExtenderRegisterTypes(types);
            //HACK here you can add validation or default class options for the new types
            foreach (Type type in types)
            {
                typesInfo.XafExtenderAddDefaultClassOptions(type);
            }

            typesInfo.XafExtenderUpdateCurrentModel(this.MetadataClient);





        }
        
    }
}
