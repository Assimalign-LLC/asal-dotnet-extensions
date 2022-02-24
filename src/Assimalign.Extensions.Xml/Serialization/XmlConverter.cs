using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Xml.Serialization
{

    public abstract class XmlConverter
    {

    }


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class XmlConverter<T> : XmlConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="type"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public abstract T? Read(ref XmlReader reader, Type type, XmlSerializerOptions options);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public abstract void Write(XmlWriter writer, T value, XmlSerializerOptions options);
    }
}
