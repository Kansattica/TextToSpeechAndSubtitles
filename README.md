# TextToSpeechAndSubtitles

should work like this:

`dotnet run somefile.txt someotherfile.txt`

and that'll create `somefile.wav` and `somefile.ass` and the same for any other files you specify.

Only works on Windows because it uses the Windows System.Speech library.

There's a weird bug with the Windows speech synth where it only generates correct timings if you let it read it out loud to the primary speakers. So, this reads it out loud (sorry about that), generates a subtitle file with correct timings, then makes the wav file right after. For some reason this generates a subtitle file with timings that match the wav file. 