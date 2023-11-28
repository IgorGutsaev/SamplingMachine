namespace FutureTechniksProtocols
{
    public abstract class VmcAcknowledgementEventArgs : EventArgs
    {
        public byte[] Response { get; set; }
    }

    public class HandshakeAckEventArgs : VmcAcknowledgementEventArgs { }

    public class MotorRunAckEventArgs : VmcAcknowledgementEventArgs { }

    public class DispensingAckEventArgs : VmcAcknowledgementEventArgs
    {
        public bool Dispensed { get; set; }

        public int MotorId { get; set; }
    }

    public class DoorStateAckEventArgs : VmcAcknowledgementEventArgs
    {
        /// <summary>
        /// The door is closed
        /// </summary>
        public bool Closed { get; set; }
    }
}