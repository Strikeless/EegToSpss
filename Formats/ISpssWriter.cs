namespace EegToSpss.Formats
{
    internal interface ISpssWriter
    {
        public void WriteSpss(GlobalData data, StreamWriter writer, string fieldPrefix);
    }
}
