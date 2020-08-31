
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;
using System;
using System.Linq;

namespace BIT.XpoExtender.Client
{
    public static class XpoHelper
    {

        static System.Reflection.Assembly assembly;
        public static void InitXpo(string connectionString)
        {
            assembly = typeof(XpoHelper).Assembly;
            var dictionary = PrepareDictionary();

            if (XpoDefault.DataLayer == null)
            {

                using (var updateDataLayer = XpoDefault.GetDataLayer(connectionString, dictionary, AutoCreateOption.DatabaseAndSchema))
                {
                    updateDataLayer.UpdateSchema(false, dictionary.CollectClassInfos(assembly));
                }
            }

            var dataStore = XpoDefault.GetConnectionProvider(connectionString, AutoCreateOption.SchemaAlreadyExists);
            XpoDefault.DataLayer = new ThreadSafeDataLayer(dictionary, dataStore);
            XpoDefault.Session = null;


        }
        public static UnitOfWork CreateUnitOfWork()
        {
            return new UnitOfWork();
        }
        static XPDictionary PrepareDictionary()
        {
            var dict = new ReflectionDictionary();
            dict.GetDataStoreSchema(assembly);
            return dict;
        }



    }
}
