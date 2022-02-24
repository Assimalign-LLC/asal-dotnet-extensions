using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Buffers;

namespace Assimalign.Extensions.Xml
{
    public ref struct XmlReader
    {
        private SequenceReader<byte> reader;
        private ReadOnlySequence<byte> sequence;


        private const byte TagOpen = (byte)'<';
        private const byte TagClose = (byte)'>';


        public XmlReader(ReadOnlySpan<byte> content)
        {
            this.sequence = new ReadOnlySequence<byte>(content.ToArray());
            this.reader = new SequenceReader<byte>(sequence);
            this.isOnTagClose = false;
            this.isOnTagOpen = false;
        }


        bool isOnTagOpen;
        bool isOnTagClose;


        public bool Read()
        {
            // Check if a the beginning of the DOM
            if (reader.Consumed == 0)
            {
                if (reader.TryReadTo(out ReadOnlySequence<byte> sequence, TagOpen))
                {
                    goto IL_016c;
                }
                else
                {
                    // TODO: Create Invlid DOM Format Exception
                    throw new Exception();
                }
            }
            else
            {

            }



            IL_016c:






            throw new NotImplementedException();
        }

        private bool ReadTag()
        {

        }





        /// <summary>
        /// Resets the position of the reader back to the beginning.
        /// </summary>
        public void Reset() =>
             reader.Rewind(reader.Consumed);
    }
}
