using Gpio;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Main
{
    class Program
    {
        #region Constants

        private const long WaitForDebuggerTimeout = 60000;

        #endregion

        #region Fields

        private static Led _led5;
        private static BlinkService _led5BlinkingService;

        #endregion

        #region Main Method

        static async Task Main()
        {
            WaitForDebugger();

            using GpioController gpio = new GpioController();
            using var button = new Button(gpio, 6);
            using var buttonPressService = new ButtonPressService(button);
            buttonPressService.Clicked += ButtonClicked;
            buttonPressService.LongPress += ButtonLongPress;
            // button.Pressed += Button_Pressed;
            // button.Released += Button_Released;

            _led5 = new Led(gpio, 5);
            _led5BlinkingService = new BlinkService(_led5);

            await Task.Delay(30000);
        }

        #endregion

        #region Private Methods

        #region Event Handlers

        private static void ButtonClicked(object sender, EventArgs e)
        {
            if (_led5.State)
            {
                _led5.Off();
            }
            else
            {
                _led5.On();
            }
        }

        private static void ButtonLongPress(object sender, EventArgs e)
        {
            if (_led5BlinkingService.IsBlinking)
            {
                _led5BlinkingService.Stop();
            }
            else
            {
                _led5BlinkingService.Start();
            }
        }

        private static void Button_Pressed(object sender, EventArgs e)
        {
            Console.WriteLine("Pressed");

            _led5.On();
        }

        private static void Button_Released(object sender, EventArgs e)
        {
            Console.WriteLine("Released");

            _led5.Off();
        }

        #endregion

        private static void WaitForDebugger()
        {
#if DEBUG
            Stopwatch sw = new Stopwatch();
            sw.Start();

            // wait for a debugger to attach.
            while (!Debugger.IsAttached)
            {
                // After WaitForDebuggerTimeout milliseconds, exit the loop.
                // This will keep the app from running forever.
                if (sw.ElapsedMilliseconds >= WaitForDebuggerTimeout)
                {
                    sw.Stop();
                    Environment.Exit(1);
                }

                Thread.Sleep(10);
            }

            sw.Stop();
#endif
        }

        #endregion
    }
}
