using System.Collections.Concurrent;

namespace Registeration.Main.Application.Queues
{
    public class VerificationCodeQueue
    {
        private readonly ConcurrentQueue<VerificationCodeMessage> _queue = new();
        private readonly SemaphoreSlim _signal = new(0);

        public void Enqueue(VerificationCodeMessage message)
        {
            _queue.Enqueue(message);
            _signal.Release(); // Signal the worker to process the queue
        }

        public async Task<VerificationCodeMessage?> DequeueAsync()
        {
            await _signal.WaitAsync();
            _queue.TryDequeue(out var message);
            return message;
        }
    }
}
