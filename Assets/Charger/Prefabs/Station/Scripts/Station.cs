using System;
using UnityEngine;

namespace Charger.Station
{
    [RequireComponent(typeof(Renderer))]
    public class Station : MonoBehaviour, IDimmerable
    {
        public event Action EmissionReachedMax;
        public event Action EmissionReachedZero;

        [Header("Dim")]
        [SerializeField] private string _emissionKeyword = "_EmissionColor";
        [SerializeField] private Color _emissionColor;
        [SerializeField, Range(0f, 1f)] private float _startEmission = 1f;
        [SerializeField, Min(0f)] private float _dimRate = 0.1f;
        
        private Renderer _renderer;
        private float _emission;

        float IDimmerable.Emission => _emission;

        float IDimmerable.DimRate
        {
            get => _dimRate;
            set
            {
                if (_dimRate < 0)
                    throw new ArgumentOutOfRangeException(nameof(value));
                _dimRate = value;
            }
        }

        void IDimmerable.AddEmission(float value)
        {
            if (value < 0 || value > 1f) 
                throw new ArgumentOutOfRangeException(nameof(value));
            if (_emission + value > 1f)
                value = 1f - _emission;
            SetEmission(_emission + value);
        }

        public void ReduceEmission(float value)
        {
            if (value < 0 || value > 1f)
                throw new ArgumentOutOfRangeException(nameof(value));
            if (_emission - value < 0f)
                value = _emission;
            SetEmission(_emission - value);
        }

        private void Start()
        {
            _emission = _startEmission;
            _renderer = GetComponent<Renderer>();
        }

        private void FixedUpdate()
        {
            ReduceEmission(Time.fixedDeltaTime * _dimRate);
        }

        private void SetEmission(float value)
        {
            _emission = value;
            Color color = _emissionColor * _emission;
            _renderer.material.SetColor(_emissionKeyword, color);
        }
    }
}
