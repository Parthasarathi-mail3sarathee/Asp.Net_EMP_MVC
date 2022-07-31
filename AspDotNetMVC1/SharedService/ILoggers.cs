namespace AspDotNetMVC1.SharedService
{
    public interface ILoggers
    {
        void WriteLog(string msg);
        void WriteLogComplete();
        void setFileLog(string filepath);

    }
}
