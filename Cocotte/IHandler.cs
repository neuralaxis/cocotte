namespace Cocotte
{
    public interface IHandler<T>
    {
        void Handle(T msg);
    }
}
