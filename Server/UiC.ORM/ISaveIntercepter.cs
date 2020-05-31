namespace UiC.ORM
{
    public interface ISaveIntercepter
    {
        void BeforeSave(bool insert);
    }
}