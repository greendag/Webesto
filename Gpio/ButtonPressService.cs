using System;
using System.Threading;

namespace Gpio
{
    public class ButtonPressService : IDisposable
    {
        #region Constants

        private const int MinimumTimeForLongPress = 1000;

        #endregion

        #region Fields

        private readonly Button _button;
        private readonly Timer _timer;
        private bool _isLongPress;

        #endregion

        #region Properties

        public bool IsDisposed {get; private set; }

        #endregion

        #region Events

        public event EventHandler Clicked;
        public event EventHandler LongPress;

        #region Invocators

        protected virtual void OnClicked()
        {
            Clicked?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnLongPress()
        {
            LongPress?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #endregion

        #region Constructor

        public ButtonPressService(Button button)
        {
            _button = button ?? throw new ArgumentNullException(nameof(button));

            _button.Pressed += ButtonPressed;
            _button.Released += ButtonReleased;

            _timer = new Timer(LongPressTimeMet);
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
                    _timer.Dispose();
                }

                IsDisposed = true;
            }
        }

        #endregion

        #region Private Methods

        #region Event Handler

        private void ButtonPressed(object sender, EventArgs e)
        {
            _timer.Change(MinimumTimeForLongPress, Timeout.Infinite);
        }

        private void ButtonReleased(object sender, EventArgs e)
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);

            if (!_isLongPress)
            {
                OnClicked();
            }

            _isLongPress = false;
        }

        #endregion

        private void LongPressTimeMet(object state)
        {
            _timer.Change(Timeout.Infinite, Timeout.Infinite);

            _isLongPress = true;
            OnLongPress();
        }

        #endregion
    }
}