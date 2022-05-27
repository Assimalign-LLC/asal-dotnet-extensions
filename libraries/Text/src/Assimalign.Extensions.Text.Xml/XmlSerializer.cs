using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Xml
{
    using Assimalign.Extensions.Xml.Extensions;

    /// <summary>
    /// 
    /// </summary>
    public static class XmlSerializer
    {
        private static readonly XmlSerializerOptions xmloptions;
        private static readonly ConcurrentDictionary<Type, XmlSerializerContext> xmlcontexts; // To reduce reflection calls on deserialization and serialization request for specified methods let's cache the context.

        static XmlSerializer()
        {
            xmlcontexts = new ConcurrentDictionary<Type, XmlSerializerContext>();

            xmloptions ??= new XmlSerializerOptions();
            xmloptions.AddBuiltInConverters();
        }



        public static string Serialize(object? instance, XmlSerializerOptions options = null)
        {
            
            if (instance is null)
            {
                return null;
            }

            throw new NotImplementedException();
        }

        public static void Serialize(object? instance, XmlSerializerOptions options = null)
        {
            throw new NotImplementedException();
        }






        public static object Deserialize()
        {

        }


    }
}
