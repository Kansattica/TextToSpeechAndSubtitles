using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextToSpeechAndSubtitles
{
    internal class SynthesisFileInfo
    {
        public string InputFileName { get; init; }
        public string SubtitleFileName { get; init; }
        public string AudioFileName { get; init; }
        public SynthesisFileInfo(string fileName)
        {
            InputFileName = fileName;

            var baseName = Path.GetFileNameWithoutExtension(fileName);

            SubtitleFileName = baseName + ".ass";
            AudioFileName = baseName + ".wav";
        }
    }
}
