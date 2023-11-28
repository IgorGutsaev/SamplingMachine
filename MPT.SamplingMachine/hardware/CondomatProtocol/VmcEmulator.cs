namespace FutureTechniksProtocols
{
    /// <summary>
    /// Emulated dispenser
    /// </summary>
    public class VmcEmulator : IDispenser
    {
        public event EventHandler<DataEventArgs> onEvent;
        public event EventHandler<HandshakeAckEventArgs> onHandshake;
        public event EventHandler<MotorRunAckEventArgs> onMotorRun;
        public event EventHandler<DispensingAckEventArgs> onDispensing;
        public event EventHandler<DoorStateAckEventArgs> onDoorState;

        public async Task InitializeAsync()
            => await Task.Delay(1000);

        public void CheckDoorState() {
            onEvent?.Invoke(this, new DataEventArgs { Response = new byte[3] { 0xAA, 0x55, 0xFF }, Comment = "Door status", IsCommand = true });
            Task.Delay(1000);
            onDoorState?.Invoke(this, new DoorStateAckEventArgs { Closed = true });
        }

        public void SendExtract(int motorId) {
            if (motorId < 1 || motorId > 12) {
                Console.WriteLine("Invalid motor number!");
                return;
            }

            onEvent?.Invoke(this, new DataEventArgs { Response = new byte[3] { 0xAA, 0x55, _motorIdToByte(motorId) }, Comment = $"Dispense from {motorId}", IsCommand = true });
            Task.Delay(3000);
            onDispensing?.Invoke(this, new DispensingAckEventArgs {
                Dispensed = true,
                MotorId = motorId
            });
        }

        public async Task SendExtractAsync(IEnumerable<int> motorIds) {
            if (motorIds.Any(x => x < 1 || x > 12)) {
                Console.WriteLine("Invalid motor number!");
                return;
            }

            foreach (int motorId in motorIds) {
                byte[] command = new byte[3] { 0xAA, 0x55, _motorIdToByte(motorId) };
                onEvent?.Invoke(this, new DataEventArgs { Response = command, Comment = $"Dispense from {motorId}", IsCommand = true });
                await Task.Delay(3000);
                onDispensing?.Invoke(this, new DispensingAckEventArgs {
                    Dispensed = true,
                    MotorId = motorId
                });
            }
        }

        public void Close() { }

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
    }
}