using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstrumentTracking
{
    public interface IMotorController
    {
        void MoveRelative(float displacement, bool waitTillDone);
        void MoveAbsolute(float position, bool waitTillDone);
        float GetPosition();

    }
}
