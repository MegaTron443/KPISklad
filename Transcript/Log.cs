public static class Log
{
    private static List<AbstractTranscript> transcripts;

    static Log()
    {
        transcripts = new List<AbstractTranscript>();
    }

    public static void AddTranscript(AbstractTranscript transcript)
    {
        transcripts.Add(transcript);
    }

    public static void PrintTranscripts()
    {
        foreach (var transcript in transcripts)
        {            
            Console.WriteLine(transcript.ToString());
        }
    }

    public static void ClearTranscripts()
    {
        transcripts.Clear();
    }

    public static void PrintFromDate(DateTime from, DateTime to)
    {
        foreach (var transcript in transcripts)
        {
            if (transcript.DateTime >= from && transcript.DateTime <= to)
            {
                Console.WriteLine(transcript.ToString());
            }
        }
    }
}