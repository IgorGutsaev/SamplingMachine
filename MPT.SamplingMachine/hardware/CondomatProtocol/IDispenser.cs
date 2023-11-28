namespace FutureTechniksProtocols
{
    public interface IDispenser
    {
        event EventHandler<DataEventArgs> onEvent;
        event EventHandler<HandshakeAckEventArgs> onHandshake;
        event EventHandler<MotorRunAckEventArgs> onMotorRun;
        event EventHandler<DispensingAckEventArgs> onDispensing;
        event EventHandler<DoorStateAckEventArgs> onDoorState;

        Task InitializeAsync();
        void CheckDoorState();
        void SendExtract(int motorId);
        Task SendExtractAsync(IEnumerable<int> motorIds);
        void Close();
    }
}