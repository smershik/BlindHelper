using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Speech.Synthesis;
using System.Text;
using System.Windows.Media.Media3D;

namespace BlindHelper.Models {

    using DataModel.Readers;    

    internal sealed class BarrierChecker {

        private DataProvider dataProvider;
        SpeechSynthesizer synth;

        private bool[,] recognizedData;

        internal BarrierChecker(DataProvider dataProvider) {
            this.dataProvider = dataProvider;
            synth = new SpeechSynthesizer();
            synth.SetOutputToDefaultAudioDevice();
            synth.Volume = 100;
            recognizedData = new bool[3, 3];
        }        

        public bool[,] Check() {

            ResetRecognizedData();

            byte[] nextRawFrameData;
            dataProvider.GetNextRawFrameTo(out nextRawFrameData);

            foreach (var point in PreparedData(nextRawFrameData)) {
                if (point.Z > 1000 && point.Z < 1100) {
                    ClassifyRegion(point.X, point.Y);
                }
            }
            return recognizedData;
        }

        public void Speak() {
            if (synth.State == SynthesizerState.Speaking) {
                return;
            }

            StringBuilder sb = new StringBuilder();

            if (recognizedData[0, 0]) { sb.Append("один, "); }
            if (recognizedData[0, 1]) { sb.Append("два, "); }
            if (recognizedData[0, 2]) { sb.Append("три, "); }

            if (recognizedData[1, 0]) { sb.Append("четыре, "); }
            if (recognizedData[1, 1]) { sb.Append("пять, "); }
            if (recognizedData[1, 2]) { sb.Append("шесть, "); }

            if (recognizedData[2, 0]) { sb.Append("семь, "); }
            if (recognizedData[2, 1]) { sb.Append("восемь, "); }
            if (recognizedData[2, 2]) { sb.Append("девять, "); }

            if (sb.Length > 0) { sb.Insert(0, "Помеха, квадрат: "); }

            synth.SpeakAsync(sb.ToString());
        }

        private void ResetRecognizedData() {
            for (int y = 0; y < recognizedData.GetLength(0); ++y) {
                for (int x = 0; x < recognizedData.GetLength(1); ++x) {
                    recognizedData[y, x] = false;
                }
            }
        }

        private void ClassifyRegion(double x, double y) {

            int regionX = ClassifyN(dataProvider.FrameInfo.Width, x);
            int regionY = ClassifyN(dataProvider.FrameInfo.Height, y);

            recognizedData[regionY, regionX] = true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int ClassifyN(int vectorLength, double n) {

            double thirdN = (vectorLength - 1.0) / 3.0;

            if (n >= 0 && n < thirdN) {
                return 0;
            }
            else if (n >= thirdN && n < thirdN * 2) {
                return 1;
            }
            else if (n >= thirdN * 2 && n < dataProvider.FrameInfo.Width) {
                return 2;
            }
            throw new IndexOutOfRangeException();
        }

        private IEnumerable<Point3D> PreparedData(byte[] nextRawFrameData) {

            Point3D result = new Point3D(0.0, 0.0, 0.0);

            for (int i = 0; i < dataProvider.FrameInfo.Length; ++i) {

                int x = i % 640;
                int y = i / 640;
                int z = GetDepthFromRawFrameAt(nextRawFrameData, i * sizeof(short));

                result.X = x;
                result.Y = y;
                result.Z = z;

                yield return result;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private int GetLinearIndex(int x, int y, int width) {
            return width * y + x;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private short GetDepthFromRawFrameAt(byte[] array, int index) {
            short result = array[index], lowByte = array[index + 1];
            result <<= 8; result |= lowByte;                         // <-- depth short construct
            return result >>= 3;                                     // <-- remove 3 unused low bits & return
        }
    }
}