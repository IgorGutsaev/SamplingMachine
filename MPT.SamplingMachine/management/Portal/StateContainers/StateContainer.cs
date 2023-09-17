using MPT.Vending.API.Dto;

namespace Portal.StateContainers
{
    public abstract class StateContainer<T> where T : class
    {
        public event Action OnStateChange;

        public T Value { get; set; }

        public void SetValue(T value)
        {
            Value = value;
            NotifyStateChanged();
        }

        private void NotifyStateChanged()
            => OnStateChange?.Invoke();
    }
}