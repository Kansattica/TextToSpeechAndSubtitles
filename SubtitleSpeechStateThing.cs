﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Speech.AudioFormat;
using System.Speech.Synthesis;

namespace TextToSpeechAndSubtitles
{
    internal class SubtitleSpeechStateThing
    {
        private List<SubtitleUnit> _subtitleUnits = new List<SubtitleUnit>();
        private SynthesisFileInfo _fileInfo;


        public SubtitleSpeechStateThing(SynthesisFileInfo fileInfo)
        {
            _fileInfo = fileInfo; 
        }

        public void DoSynthesisAndSubtitles()
        {
            using (SpeechSynthesizer synth = new SpeechSynthesizer())
            {
                //synth.SetOutputToWaveFile(fileInfo.AudioFileName, new SpeechAudioFormatInfo(32000, AudioBitsPerSample.Sixteen, AudioChannel.Mono));
                synth.SetOutputToDefaultAudioDevice();

                synth.SelectVoiceByHints(VoiceGender.Female);

                synth.SpeakProgress += synth_SpeakProgress;

                var builder = new PromptBuilder();

                var inputLines = File.ReadAllLines(_fileInfo.InputFileName);

                for (int i = 0; i < inputLines.Length; i++)
                {
                    builder.AppendText(inputLines[i]);
                }

                synth.Speak(builder);

                using (var fileStream = File.Open(_fileInfo.SubtitleFileName, FileMode.Create))
                {
                    //    subWriter.WriteStream(fileStream, subtitleItems);
                    fileStream.Write(Encoding.UTF8.GetBytes(
                        @"[Script Info]
; Script generated by Princess Grace's TextToSpeechAndSubtitles
ScriptType: v4.00
WrapStyle: Smart

[Events]
Format: Layer, Start, End, Style, Name, MarginL, MarginR, MarginV, Effect, Text
"
                        ));

                    foreach (var unit in _subtitleUnits)
                    {
                        var startTimestamp = unit.StartTime.ToString(@"h\:mm\:ss\.ff");
                        var endTimestamp = unit.EndTime.ToString(@"h\:mm\:ss\.ff");
                        fileStream.Write(Encoding.UTF8.GetBytes($"Dialogue: 0,{startTimestamp},{endTimestamp},Default,,0,0,0,,{unit.Text}\r\n"));
                    }
                }

            }

            void synth_SpeakProgress(object sender, SpeakProgressEventArgs e)
            {
                Console.WriteLine("CharPos: {0}   CharCount: {1}   AudioPos: {2}    \"{3}\"",
                  e.CharacterPosition, e.CharacterCount, e.AudioPosition, e.Text);

                TimeSpan pos = e.AudioPosition;

                var lastSub = _subtitleUnits.LastOrDefault();
                if (lastSub is not null)
                {
                    lastSub.EndTime = pos;
                }

                _subtitleUnits.Add(new SubtitleUnit
                {
                    StartTime = pos,
                    EndTime = TimeSpan.MaxValue,
                    Text = e.Text
                });
            }

        }

        public void MakeWavFile()
        {
            using (SpeechSynthesizer synth = new SpeechSynthesizer())
            {
                synth.SetOutputToWaveFile(_fileInfo.AudioFileName, new SpeechAudioFormatInfo(32000, AudioBitsPerSample.Sixteen, AudioChannel.Mono));

                synth.SelectVoiceByHints(VoiceGender.Female);

                var builder = new PromptBuilder();

                var inputLines = File.ReadAllLines(_fileInfo.InputFileName);

                for (int i = 0; i < inputLines.Length; i++)
                {
                    builder.AppendText(inputLines[i]);
                }

                synth.Speak(builder);
            }

        }

        class SubtitleUnit
        {
            public TimeSpan StartTime;
            public TimeSpan EndTime;
            public string Text = string.Empty;
        }
    }
}
