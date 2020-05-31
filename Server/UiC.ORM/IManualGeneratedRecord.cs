using UiC.ORM.SubSonic.DataProviders;
using UiC.ORM.SubSonic.Schema;

namespace UiC.ORM
{
    public interface IManualGeneratedRecord
    {
        ITable GetTableInformation(IDataProvider provider); 
    }
}