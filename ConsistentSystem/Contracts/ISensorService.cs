using ConsistentSystem.Common.Models;
using System.ServiceModel;

namespace ConsistentSystem.Contracts
{
    [ServiceContract]
    public interface ISensorService
    {
        [OperationContract]
        Measurement GetLastMeasurement();

        [OperationContract]
        void Align(double value);

        [OperationContract]
        string GetSensorName();
    }
}
