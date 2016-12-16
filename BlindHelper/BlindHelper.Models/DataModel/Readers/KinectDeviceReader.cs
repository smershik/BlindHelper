using System;


namespace BlindHelper.Models.DataModel.Readers {

    using Adapters;

    internal sealed class KinectDeviceReader : DeviceReader, IDisposable {

        public KinectDeviceReader() {
            adapter = new KinectDataAdapter(this);
        }
    }
}