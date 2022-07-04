
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Security.Core.Services;
public class HashService : IHashService
{

    public void CreateHash(string valueToHash, out byte[] valueHash, out byte[] valueSalt)
    {
        using var hmac = new HMACSHA512();

        valueSalt = hmac.Key;
        valueHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(valueToHash));
    }

    public bool VerifyHash(string valueToVerify, byte[] valueHash, byte[] valueSalt)
    {
        using HMACSHA512 hmac = new HMACSHA512(valueSalt);

        byte[] computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(valueToVerify));

        return computedHash.SequenceEqual(valueHash);
    }
}
