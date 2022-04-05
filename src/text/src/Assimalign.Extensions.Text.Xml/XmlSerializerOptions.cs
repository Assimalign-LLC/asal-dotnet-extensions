
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Xml
{
    using Assimalign.Extensions.Xml.Serialization;

    /// <summary>
    /// 
    /// </summary>
    public sealed class XmlSerializerOptions
    {
        private readonly IList<XmlConverter> converters;

        public XmlSerializerOptions()
        {
            this.converters = new List<XmlConverter>(); 
        }


        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<XmlConverter> Converters => converters;


        /// <summary>
        /// 
        /// </summary>
        public bool WriteIndents { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool ElementNameCaseInsensitive { get; set; }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="converter"></param>
        public void AddConverter(XmlConverter converter)
        {
            this.converters.Add(converter);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="converters"></param>
        public void AddConverters(IEnumerable<XmlConverter> converters)
        {
            foreach(var converter in converters)
            {
                this.converters.Add(converter);
            }
        }
    }
}
