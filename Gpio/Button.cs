using System;
using System.Device.Gpio;

namespace Gpio
{
    public class Button : GpioPin
    {
        #region Events

        public event EventHandler Pressed;
        public event EventHandler Released;

        #region Invocators

        protected virtual void OnPressed()
        {
            Pressed?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnReleased()
        {
            Released?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #endregion

        #region Constructors

        public Button(System.Device.Gpio.GpioController gpioController, int pin, PinMode pinMode = PinMode.InputPullUp) : base(gpioController, pin)
        {
            if (pinMode == PinMode.Output)
            {
                throw new ArgumentOutOfRangeException(nameof(pinMode), $"{nameof(pinMode)} must be an input type.");
            }

            GpioController.OpenPin(Pin, pinMode);
            GpioController.RegisterCallbackForPinValueChangedEvent(Pin, PinEventTypes.Rising, OnPinRising);
            GpioController.RegisterCallbackForPinValueChangedEvent(Pin, PinEventTypes.Falling, OnPinFalling);
        }

        #endregion

        #region Private Methods

        private void OnPinRising(object sender, PinValueChangedEventArgs args)
        {
            OnReleased();
        }

        private void OnPinFalling(object sender, PinValueChangedEventArgs args)
        {
            OnPressed();
        }

        #endregion
    }
}
