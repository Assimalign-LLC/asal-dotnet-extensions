using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Xml.Serialization
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(
        AttributeTargets.Class |
        AttributeTargets.Enum |
        AttributeTargets.Struct |
        AttributeTargets.Property |
        AttributeTargets.Field,
        AllowMultiple = false)]
    public sealed class XmlConverterAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <exception cref="ArgumentException">Will throw if type does not inherit from 'XmlConverter'.</exception>
        public XmlConverterAttribute(Type type)
        {
            if (type.IsAssignableTo(typeof(XmlConverter)))
            {
                //type.GetConstructors().FirstOrDefault(x => x.)
            }
            else
            {
                throw new ArgumentException("");
            }
        }



        internal XmlConverter Converter { get; }
    }
}
