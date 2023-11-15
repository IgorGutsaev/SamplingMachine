using System.IO.Ports;
using System.Runtime.CompilerServices;

namespace CondomatProtocol
{
   /// <summary>
   /// VMC service (Vending machine controller)
   /// </summary>
    public class CondomatCommunicationService
    {
        public event EventHandler<DataEventArgs> onEvent;

        public CondomatCommunicationService(int portNumber) {
            if (_port == null) {
                DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 1);
                defaultInterpolatedStringHandler.AppendLiteral("COM");
                defaultInterpolatedStringHandler.AppendFormatted(portNumber);
                _port = new SerialPort(defaultInterpolatedStringHandler.ToStringAndClear(), 9600);
                _port.DataReceived += (sender, e) => {
                    byte[] data = new byte[_port.BytesToRead];
                    _port.Read(data, 0, data.Length);
                    onEvent?.Invoke(this, new DataEventArgs { Response = data });
                    busy = false;
                };
                ////_port.Open();
            }
        }

        public async Task Initialize() {
            byte[] command = new byte[3] { 0xAA, 0x55, 0xFF };
            onEvent?.Invoke(this, new DataEventArgs { Response = command, Comment = "Initialization", IsCommand = true });
            _port.Write(command, 0, 3);
        }

        public void CheckDoorState() {
            byte[] command = new byte[3] { 0xAA, 0x55, 0xC9 };
            onEvent?.Invoke(this, new DataEventArgs { Response = command, Comment = "Door status", IsCommand = true });
            _port.Write(command, 0, 3);
        }

        public void SendExtract(int motorId) {
            if (motorId < 1 || motorId > 12) {
                Console.WriteLine("Invalid motor number!");
                return;
            }

            byte[] command = new byte[3] { 0xAA, 0x55, (byte)motorId };
            onEvent?.Invoke(this, new DataEventArgs { Response = command, Comment = $"Dispense from {motorId}", IsCommand = true });
            _port.Write(command, 0, 3);
        }

        public async Task SendExtractAsync(IEnumerable<int> motorIds) {
            if (motorIds.Any(x => x < 1 || x > 12)) {
                Console.WriteLine("Invalid motor number!");
                return;
            }

            foreach (int motorId in motorIds) {
                byte[] command = new byte[3] { 0xAA, 0x55, (byte)motorId };
                onEvent?.Invoke(this, new DataEventArgs { Response = command, Comment = $"Dispense from {motorId}", IsCommand = true });
                busy = true;
                _port.Write(command, 0, 3);
                while (busy) {
                    await Task.Delay(1000);
                }
            }
        }

        public void Close() => _port.Close();

        private readonly SerialPort _port;
        private bool busy = false;
    }
}
