using DevExpress.Xpo;
using DevExpress.Xpo.Metadata;
using System;
using System.Collections.Generic;
using System.Text;

namespace BIT.XpoExtender.Client
{
    [NonPersistent()]
    public abstract class XpoExtenderBaseObject:XPObject
    {
        public XpoExtenderBaseObject()
        {

        }

        public XpoExtenderBaseObject(Session session) : base(session)
        {

        }

        public XpoExtenderBaseObject(Session session, XPClassInfo classInfo) : base(session, classInfo)
        {

        }
    }
}
