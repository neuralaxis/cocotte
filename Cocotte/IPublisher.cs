namespace Cocotte
{
    interface IPublisher
    {
        void Publish<T>(string topic, T msg);
    }
}
