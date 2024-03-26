// ----------------------------------------------------------------------------
// CSA - C# in Action
// (c) 2024, Christian Jost, HSLU
// ----------------------------------------------------------------------------
using System;
using System.Collections.Specialized;
using System.Device.Gpio;
using System.Diagnostics;
using System.Threading;

namespace Explorer700Library
{
    public class Joystick
    {
        #region members & events
        private bool running = false;
        private int centerPin;
        public event EventHandler<KeyEventArgs> JoystickChanged;
        #endregion

        #region constructor & destructor
        public Joystick(Pcf8574 pcf8574, GpioController gpioController)
        {
            Pcf8574 = pcf8574;
            pcf8574.Mask |= 0x0F;
            centerPin = 20;
            GpioController = gpioController;
            GpioController.OpenPin(centerPin, PinMode.InputPullUp);

            // Start Polling-Thread
            Thread t = new Thread(Run);
            t.IsBackground = true;
            t.Start();
        }
        #endregion

        #region properties
        private Pcf8574 Pcf8574 { get; set; }

        internal GpioController GpioController { get; }

        /// <summary>
        /// Liest und liefert den Zustand des Joysticks
        /// </summary>
        public Keys Keys
        {
            get
            {
                Keys k = Keys.NoKey;
                // ToDo
                if (!(bool)GpioController.Read(centerPin)) k |= Keys.Center;
                if (!this.Pcf8574[0]) k |= Keys.Left;
                if (!this.Pcf8574[1]) k |= Keys.Up;
                if (!this.Pcf8574[2]) k |= Keys.Down;
                if (!this.Pcf8574[3]) k |= Keys.Right;
                    
                return k;
            }
        }
        #endregion

        #region methods
        /// <summary>
        /// Pollt alle 50ms den Joystick und generiert ein JoystickChanged Event, falls
        /// sich der Zustand des Joysticks (Taste gedrückt/losgelassen) verändert hat.
        /// </summary>
        private void Run()
        {
            running = true;
            Keys oldState = new();
            Keys currentState = new();
            while (running)
            {
                currentState = Keys;
                if (currentState != oldState)
                {
                    JoystickChanged?.Invoke(this, new KeyEventArgs(Keys));
                    oldState = currentState;
                }
                Thread.Sleep(50);
            }
        }
        #endregion
    }
}
