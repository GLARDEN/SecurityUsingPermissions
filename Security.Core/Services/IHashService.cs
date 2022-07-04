namespace Security.Core.Services;

public interface IHashService
{
    void CreateHash(string valueToHash, out byte[] valueHash, out byte[] valueSalt);
    bool VerifyHash(string valueToVerify, byte[] valueHash, byte[] valueSalt);
}
