using System.Windows;


namespace BlindHelper.Models.DataModel.Adapters {

    using Readers;

    internal interface IDataAdapter {

        DataProvider DataProvider { get; }

        Point[] GetAdapted(byte[] rawFrameBuffer);
    }
}