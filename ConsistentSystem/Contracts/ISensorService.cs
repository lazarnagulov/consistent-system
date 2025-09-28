using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ConsistentSystem.Contracts
{
    [ServiceContract]
    public interface ISensorService
    {
        [OperationContract]
        double GetLastMeasurement();

        [OperationContract]
        void Align(double value);

        [OperationContract]
        string GetSensorName();
    }
}
