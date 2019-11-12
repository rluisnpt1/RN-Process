namespace RN_Process.Shared.Commun
{
    public interface IValidatorStrategy<T>
    {
        bool IsValid(T validateThis);
    }
}