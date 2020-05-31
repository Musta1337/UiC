using System;

namespace UiC.ORM.SubSonic.DataProviders.Log
{
    public interface ILogAdapter
    {
        void Log(String message);
    }
}