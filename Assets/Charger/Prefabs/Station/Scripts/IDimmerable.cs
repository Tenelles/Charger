using System;

namespace Charger.Station
{
    public interface IDimmerable
    {
        float Emission { get; }
        float DimRate { get; set; }
        
        void AddEmission(float value);
        void ReduceEmission(float value);
    }
}