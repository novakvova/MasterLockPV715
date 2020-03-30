using MasterLock.DTO;
using MasterLock.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MasterLock.Abstract
{
    public interface IJWTTokenService
    {
        string CreateToken(DbUser user);
        string CreateRefreshToken(DbUser user);
        Task<TokensDTO> RefreshAuthToken(string oldAuthToken, string refreshToken);
    }
}
