using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assimalign.Extensions.Mapping;

/// <summary>
/// 
/// </summary>
public interface IMapperProfileBuilder
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TTarget"></typeparam>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="configure"></param>
    /// <returns></returns>
    IMapperProfileBuilder CreateProfile<TTarget, TSource>(Action<IMapperActionDescriptor<TTarget, TSource>> configure);

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerable<IMapperProfile> Build();
}
