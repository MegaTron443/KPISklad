public static class Log
{
    private static readonly List<AbstractTranscript> Transcripts;

    static Log()
    {
        Transcripts = [];
    }

    public static void AddTranscript(AbstractTranscript transcript)
    {
        Transcripts.Add(transcript);
    }

    public static void PrintTranscripts()
    {
        foreach (var transcript in Transcripts)
        {
            Console.WriteLine(transcript.ToString());
        }
    }

    public static void ClearTranscripts()
    {
        Transcripts.Clear();
    }

    public static void PrintFromDate(DateTime from, DateTime to)
    {
        foreach (var transcript in Transcripts)
        {
            if (transcript.DateTime >= from && transcript.DateTime <= to)
            {
                Console.WriteLine(transcript.ToString());
            }
        }
    }
}
