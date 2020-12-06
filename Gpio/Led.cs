using System;
using System.Device.Gpio;

namespace Gpio
{
    public class Led : GpioPin
    {
        #region Properties

        public bool State { get; private set; }

        #endregion

        #region Constructors

        public Led(System.Device.Gpio.GpioController gpioController, int pin) : base (gpioController, pin)
        {
            GpioController.OpenPin(Pin, PinMode.Output);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Turns on the led.
        /// </summary>
        public void On()
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(nameof(Led));
            }

            GpioController.Write(Pin, PinValue.High);
            State = true;
        }

        /// <summary>
        /// Turns off the led.
        /// </summary>
        public void Off()
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(nameof(Led));
            }

            GpioController.Write(Pin, PinValue.Low);
            State = false;
        }

        #endregion
    }
}
