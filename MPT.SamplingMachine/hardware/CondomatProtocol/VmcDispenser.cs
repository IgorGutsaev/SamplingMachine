using System.IO.Ports;
using System.Runtime.CompilerServices;

namespace FutureTechniksProtocols
{
   /// <summary>
   /// VMC service (Vending machine controller)
   /// </summary>
    public class VmcDispenser : IDispenser
    {
        public event EventHandler<DataEventArgs> onEvent;
        public event EventHandler<HandshakeAckEventArgs> onHandshake;
        public event EventHandler<MotorRunAckEventArgs> onMotorRun;
        public event EventHandler<DispensingAckEventArgs> onDispensing;
        public event EventHandler<DoorStateAckEventArgs> onDoorState;

        public VmcDispenser(int portNumber) {
            if (_port == null) {
                DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(3, 1);
                defaultInterpolatedStringHandler.AppendLiteral("COM");
                defaultInterpolatedStringHandler.AppendFormatted(portNumber);
                _port = new SerialPort(defaultInterpolatedStringHandler.ToStringAndClear(), 9600);
                _port.DataReceived += (sender, e) => {
                    byte[] data = new byte[_port.BytesToRead];
                    _port.Read(data, 0, data.Length);
                    HandleResponse(data);
                    busy = false;
                };

                if (portNumber > 0)
                    _port.Open();
            }
        }

        public async Task InitializeAsync() {
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
            else Console.WriteLine($"Extract from {motorId}");

            byte[] command = new byte[3] { 0xAA, 0x55, _motorIdToByte(motorId) };
            onEvent?.Invoke(this, new DataEventArgs { Response = command, Comment = $"Dispense from {motorId}", IsCommand = true });
            _port.Write(command, 0, 3);
        }

        public async Task SendExtractAsync(IEnumerable<int> motorIds) {
            if (motorIds.Any(x => x < 1 || x > 12)) {
                Console.WriteLine("Invalid motor number!");
                return;
            }
            else Console.WriteLine($"Extract from {motorIds.Count()} adress(es)");

            foreach (int motorId in motorIds) {
                byte[] command = new byte[3] { 0xAA, 0x55, _motorIdToByte(motorId) };
                onEvent?.Invoke(this, new DataEventArgs { Response = command, Comment = $"Dispense from {motorId}", IsCommand = true });
                busy = true;
                _port.Write(command, 0, 3);
                while (busy) { // do not dispense the next product until current one is dispensed
                    await Task.Delay(1000);
                }
            }
        }

        public void Close() => _port.Close();

        private void HandleResponse(byte[] data) {
            if (data.Length == 0)
                return;

            char head = data[0].ToString("x2")[0];
            switch (head) {
                case '3':
                    onDispensing?.Invoke(this, new DispensingAckEventArgs {
                        Dispensed = data[1] == 0x4F,
                        MotorId = _byteToMotorId_FromExtractionEvent(data[0]),
                        Response = data
                    });
                    break;
                case '4':
                    onMotorRun?.Invoke(this, new MotorRunAckEventArgs { Response = data });
                    break;
                case '5':
                    onHandshake?.Invoke(this, new HandshakeAckEventArgs { Response = data});
                    break;
                case '9':
                    onDoorState?.Invoke(this, new DoorStateAckEventArgs { Closed = data[0] == 0x9B });
                    break;
            }
        }

        private byte _motorIdToByte(int motorId) {
            switch (motorId) {
                case 1: return 0x01;
                case 2: return 0x02;
                case 3: return 0x03;
                case 4: return 0x04;
                case 5: return 0x05;
                case 6: return 0x06;
                case 7: return 0x07;
                case 8: return 0x08;
                case 9: return 0x09;
                case 10: return 0x10;
                case 11: return 0x11;
                case 12: return 0x12;
                default: return 0x00;
            }
        }

        /// <summary>
        /// (3X 4F 4B)- product was dispensed, (3X 4E 4B)- product wasn't dispensed, X- motor number: 1-9, A = 10, B = 11, C = 12
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        private int _byteToMotorId_FromExtractionEvent(byte b) {
            switch (b) {
                case 0x31: return 1;
                case 0x32: return 2;
                case 0x33: return 3;
                case 0x34: return 4;
                case 0x35: return 5;
                case 0x36: return 6;
                case 0x37: return 7;
                case 0x38: return 8;
                case 0x39: return 9;
                case 0x3A: return 10;
                case 0x3B: return 11;
                case 0x3C: return 12;
                default: return 0;
            }
        }

        private readonly SerialPort _port;
        private bool busy = false;
    }
}