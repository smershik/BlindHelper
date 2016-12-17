using System.Speech.Synthesis;

namespace BlindHelper.Models
{
    class SpeechProvider
    {
        SpeechSynthesizer synth;

        public enum SpeechCommands
        {
            TopLeft,
            Top,
            TopRight,
            Left,
            Center,
            Right,
            BottomRight,
            Bottom,
            BottomLeft
        }

        public SpeechProvider()
        {
            synth = new SpeechSynthesizer();
            synth.SetOutputToDefaultAudioDevice();
            synth.Volume = 100;
            
        }

        public void Allert(SpeechCommands arg)
        {
            switch (arg)
            {
                case SpeechCommands.TopLeft:
                    synth.SpeakAsync(" ");                    
                    break;
                case SpeechCommands.Top:
                    synth.SpeakAsync(" ");
                    break;
                case SpeechCommands.TopRight:
                    synth.SpeakAsync(" ");
                    break;
                case SpeechCommands.Left:
                    synth.SpeakAsync(" ");
                    break;
                case SpeechCommands.Center:
                    synth.SpeakAsync(" ");
                    break;
                case SpeechCommands.Right:
                    synth.SpeakAsync(" ");
                    break;
                case SpeechCommands.BottomLeft:
                    synth.SpeakAsync(" ");
                    break;
                case SpeechCommands.Bottom:
                    synth.SpeakAsync(" ");
                    break;
                case SpeechCommands.BottomRight:
                    synth.SpeakAsync(" ");
                    break;                
                default:
                    break;
            }
        }
    }
}
