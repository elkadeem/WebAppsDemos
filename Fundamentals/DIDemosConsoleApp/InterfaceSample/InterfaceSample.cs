using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIDemosConsoleApp.InterfaceSample
{

    public class Consumer
    {
        public void Consume()
        {
            InterfaceSample interfaceSample = new InterfaceSample();
            
            ((IServiceX)interfaceSample).Add();
            ((ServiceY)interfaceSample).Add();
            
        }
    }

    public class InterfaceSample : IServiceX, ServiceY
    {
        int IServiceX.Add()
        {
            return 5;
        }

        int ServiceY.Add()
        {
            return 10;
        }
    }


    public interface IServiceX
    {
        int Add();
    }

    public interface ServiceY
    {
        int Add();
    }
}
