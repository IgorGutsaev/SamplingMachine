namespace FutureTechniksCondomatProtocol
{
    public class DataEventArgs : EventArgs
    {
        public bool IsCommand { get; set; } = false;
        public byte[] Response { get; set; }
        public string Comment { get; set; }

        public string ResponseToString
        {
            get
            {
                if (Response != null && Response.Length > 0) {
                    string result = string.Empty;
                    foreach (byte b in Response) {
                        result += b.ToString("X2") + " ";
                    }

                    return result.Trim();
                }
                else return "No data";
            }
        }
    }
}