using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class TreatDiseaseHolder
{
    private Queue<Tuple<byte, InfectionType, byte, byte>> _diseaseCubesUpdateQueue = new();

    public void Add(byte pid, InfectionType type, byte new_count, byte loc)
    {
        _diseaseCubesUpdateQueue.Enqueue(Tuple.Create(pid, type, new_count, loc));
    }

    public Tuple<byte, InfectionType, byte, byte> GetNext()
    {
        if (_diseaseCubesUpdateQueue.Count == 0)
        {
            return null;
        }
        else
        {
            return _diseaseCubesUpdateQueue.Dequeue();
        }
    }

}