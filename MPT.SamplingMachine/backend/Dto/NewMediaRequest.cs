namespace MPT.Vending.API.Dto
{
    public class NewMediaRequest
    {
        public string Hash { get; set; }
        public string FileName { get; set; }
        public int Size { get; set; }
    }
}
