﻿using System;
using System.Threading.Tasks;

namespace BlindHelper.Models {

    using Events;
    using DataModel.Readers;

    public class Model : IDisposable {

        public event ModelUpdatedEvent OnModelUpdated;

        private DataProvider reader;
        private FramesProvider framesProvider;
        private BarrierChecker barrierChecker;

        public string CurrentState { get; private set; } = "Ready";
        public bool Ready { get; private set; } = true;
        public int FramesCount { get { return reader.TotalFrames; } }

        public Model(ModelUpdatedEvent onModelUpdated) {
            OnModelUpdated += onModelUpdated;
            MapperSwitchOffline();            
        }

        public void MapperSwitchOnline() {
            reader = new KinectDeviceReader();
            Initialize();
        }

        public void MapperSwitchOffline() {
            reader = new KinectFileReader();
            Initialize();
        }

        private void Initialize() {
            framesProvider = new FramesProvider(reader);
            barrierChecker = new BarrierChecker(reader);
        }

        private void ChangeState(string newModelState, bool lockModel = false) {
            CurrentState = newModelState;
            Ready = !lockModel;
            OnModelUpdated?.Invoke();
        }

        public Task CalculateFramesCountAsync() {
            Task calculateFramesCountTask = new Task(() => {
                ChangeState("Calculate Frames Count", true);
                (reader as FileReader)?.CalculateFramesCount();
                ChangeState("Ready");
            });
            calculateFramesCountTask.Start();
            return calculateFramesCountTask;
        }

        public bool Start(string fileName) {
            MapperSwitchOffline();
            return (bool)(reader as FileReader)?.Start(fileName);
        }

        public void MoveToPosition(int frameIndex) {
            (reader as FileReader)?.MoveToPosition(frameIndex);
        }

        public void Stop() {
            reader?.Stop();
        }

        public byte[] GetActualFullDepthFrame() {            
            return framesProvider.GetActualFrontDepthFrame();
        }

        public bool[,] GetActualBarrierRegions() {
            return barrierChecker.Check();
        }

        public void Speak() {
            barrierChecker.Speak();
        }

        public void Dispose() {
            reader?.Dispose();
        }
    }
}