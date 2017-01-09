namespace Cocotte
{
    public interface IPublisher
    {
        void Publish<T>(string topic, T msg);
    }
}
