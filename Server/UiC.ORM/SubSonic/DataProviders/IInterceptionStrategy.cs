using System;

namespace UiC.ORM.SubSonic.DataProviders
{
    public interface IInterceptionStrategy
    {
        object Intercept(object objectToIntercept);

        bool Accept(Type type);
    }

}
