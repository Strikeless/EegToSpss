namespace EegToSpss.Formats
{
    internal interface IEegReader
    {
        public void ReadEeg(ref GlobalData data, StreamReader reader, string subjectId);
    }
}
