namespace NVenter.Core {
    public interface IPositionalStreamFactory {
        IEventStream MakeStream(long position);        
    }
}