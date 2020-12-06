using System;
using System.Threading;

namespace Gpio
{
    public class BlinkService : IDisposable
    {
        #region Fields

        private readonly Led _led;
        private int _timeLedLit;
        private int _timeLedDim;
        private Timer _timer;

        #endregion

        #region Properties

        public bool IsBlinking { get; private set; }

        public bool IsDisposed { get; private set; }

        #endregion

        #region Constructor

        public BlinkService(Led led, int timeLedLit = 500, int timeLedDim = 500)
        {
            _led = led ?? throw new ArgumentNullException(nameof(led));
            _timeLedLit = timeLedLit > 0 ? timeLedLit : throw new ArgumentOutOfRangeException(nameof(timeLedLit), timeLedLit, $"The {nameof(timeLedLit)} value must be greater than zero.");
            _timeLedDim = timeLedDim > 0 ? timeLedDim : throw new ArgumentOutOfRangeException(nameof(timeLedDim), timeLedDim, $"The {nameof(timeLedDim)} value must be greater than zero.");
        }

        #endregion

        #region Public Methods

        public void Start()
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(nameof(BlinkService));
            }

            if (_timer != null)
            {
                throw new InvalidOperationException($"The Led is already blinking.");
            }

            _led.On();
            _timer = new Timer(OnBlink, true, _timeLedLit, Timeout.Infinite);
            IsBlinking = true;
        }

        public void Stop()
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException(nameof(BlinkService));
            }

            _led.Off();
            _timer?.Change(Timeout.Infinite, Timeout.Infinite);
            _timer?.Dispose();
            _timer = null;
            IsBlinking = false;
        }

        public void Change(int timeLedLit = 500, int timeLedDim = 500)
        {
            _timeLedLit = timeLedLit;
            _timeLedDim = timeLedDim;
        }

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
                    Stop();
                }

                IsDisposed = true;
            }
        }

        #endregion

        #region Private Methods

        private void OnBlink(object currentState)
        {
            if (_timer == null)
            {
                return;
            }

            if (_led.IsDisposed)
            {
                return;
            }

            if (currentState is bool ledState)
            {
                if (ledState)
                {
                    _led.Off();
                    _timer = new Timer(OnBlink, false, _timeLedDim, Timeout.Infinite);
                }
                else
                {
                    _led.On();
                    _timer = new Timer(OnBlink, true, _timeLedLit, Timeout.Infinite);
                }
            }
        }

        #endregion
    }
}