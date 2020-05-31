using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UiC.Loader.Extensions
{
    public static class ReflectionExtensions
    {
        public static bool HasInterface(this Type type, Type interfaceType)
        {
            return type.FindInterfaces(FilterByName, interfaceType).Length > 0;
        }

        private static bool FilterByName(Type typeObj, Object criteriaObj)
        {
            return typeObj.ToString() == criteriaObj.ToString();
        }
    }
}
