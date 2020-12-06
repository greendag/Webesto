using System.Device.Gpio;

namespace Gpio
{
    public class GpioController : System.Device.Gpio.GpioController
    {
        //
        // Summary:
        //     Initializes a new instance of the System.Device.Gpio.GpioController class that
        //     will use the logical pin numbering scheme as default.
        public GpioController()
        { }

        //
        // Summary:
        //     Initializes a new instance of the System.Device.Gpio.GpioController class that
        //     will use the specified numbering scheme. The controller will default to use the
        //     driver that best applies given the platform the program is executing on.
        //
        // Parameters:
        //   numberingScheme:
        //     The numbering scheme used to represent pins provided by the controller.
        public GpioController(PinNumberingScheme numberingScheme) : base (numberingScheme)
        { }

        //
        // Summary:
        //     Initializes a new instance of the System.Device.Gpio.GpioController class that
        //     will use the specified numbering scheme and driver.
        //
        // Parameters:
        //   numberingScheme:
        //     The numbering scheme used to represent pins provided by the controller.
        //
        //   driver:
        //     The driver that manages all of the pin operations for the controller.
        public GpioController(PinNumberingScheme numberingScheme, GpioDriver driver) : base(numberingScheme, driver)
        { }
    }
}
