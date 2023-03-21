namespace TextToSpeechAndSubtitles
{
    internal class Program
    {
        static void Main(string[] args)
        {
            foreach (var arg in args)
            {
                DoTextToSpeech(arg);
            }
        }


        static void DoTextToSpeech(string textFile)
        {
            if (!File.Exists(textFile))
            {
                Console.WriteLine($"Hey, buddy, I expect {textFile} to exist and be a text file. Not gonna touch it.");
                return;
            }

            var fileInfo = new SynthesisFileInfo(textFile);

            Console.WriteLine($"Going to read {fileInfo.InputFileName} and write to {fileInfo.AudioFileName} and {fileInfo.SubtitleFileName}.");

            var stateThing = new SubtitleSpeechStateThing(fileInfo);
            stateThing.DoSynthesisAndSubtitles();
            stateThing.MakeWavFile();
        }



    }
}