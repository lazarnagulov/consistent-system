using ConsistentSystem.Common.Models;
using ConsistentSystem.Sensor;
using System.ServiceModel;

namespace ConsistentSystem.Contracts
{
    [ServiceContract(CallbackContract = typeof(ISensorCallback))]
    public interface ISensorService
    {
        [OperationContract]
        Measurement GetLastMeasurement(string sensorId);

        [OperationContract(IsOneWay = true)]
        void Align(double value);

        [OperationContract]
        string GetSensorName(string sensorId);
    }

    public interface ISensorCallback
    {
        [OperationContract(IsOneWay = true)]
        void OnAlignmentStarted();

        [OperationContract(IsOneWay = true)]
        void OnAlignmentCompleted(double alignedValue);
    }
}
