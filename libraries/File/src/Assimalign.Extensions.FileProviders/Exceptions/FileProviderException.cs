using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.FileProviders;

public abstract class FileProviderException : Exception
{




    public static FileProviderException NotFound()
    {
        throw new Exception();
    }
}
