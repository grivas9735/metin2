namespace Metin2Bot
{
    public class FixedSizeQueue<T>
    {
        public readonly Queue<T> _queue = new Queue<T>();
        private readonly int _maxSize;

        public FixedSizeQueue(int maxSize)
        {
            if (maxSize <= 0)
                throw new ArgumentException("El tamaño máximo debe ser mayor que cero.", nameof(maxSize));

            _maxSize = maxSize;
        }

        public void Enqueue(T item)
        {
            _queue.Enqueue(item);
            if (_queue.Count > _maxSize)
            {
                _queue.Dequeue();
            }
        }

        public T Dequeue()
        {
            return _queue.Dequeue();
        }

        public int Count => _queue.Count;

        public T Peek()
        {
            return _queue.Peek();
        }

        public void Clear()
        {
            _queue.Clear();
        }

        public IEnumerable<T> Items => _queue;
    }
}