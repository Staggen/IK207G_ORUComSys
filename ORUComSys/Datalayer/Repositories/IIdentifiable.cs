namespace Datalayer.Repositories {
    public interface IIdentifiable<T> {
        T Id { get; set; }
    }
}