using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Text;

namespace Gpio
{
    public class GpioPin : IDisposable
    {
        #region Statics

        private static readonly List<int> _pinsInUse = new List<int>();

        #endregion

        #region Fields

        private int _pin;

        #endregion

        #region Public Properties

        protected System.Device.Gpio.GpioController GpioController { get; }

        public int Pin 
        {
            get => _pin;
            protected set 
            {
                if (_pin == value)
                    return;

                if (_pin > 0)
                {
                    throw new InvalidOperationException($"{nameof(Pin)} is a read only property.");
                }

                if (_pinsInUse.Contains(value))
                {
                    throw new InvalidOperationException($"Pin {value} is already in use.");
                }

                _pin = value;
                _pinsInUse.Add(value);
            } 
        }

        public bool IsDisposed { get; private set; }

        #endregion

        #region Constructors

        public GpioPin(System.Device.Gpio.GpioController gpioController, int pin)
        {
            GpioController = gpioController ?? throw new ArgumentNullException(nameof(gpioController));
            Pin = pin > 0 ? pin : throw new ArgumentOutOfRangeException(nameof(pin), pin, $"The pin is not valid.");
        }

        #endregion

        #region Public Methods

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Protected Methods

        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    GpioController.ClosePin(Pin);
                    _pinsInUse.Remove(Pin);
                }

                IsDisposed = true;
            }
        }

        #endregion
    }
}
