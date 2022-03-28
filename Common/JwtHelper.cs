using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class JwtHelper
    {
        /// <summary>
        /// 创建Jwt字符串
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string CreateJwt(TokenModelJwt model)
        {
            var claims = new List<Claim>
                {
                 /*
                 * 特别重要：
                   1、这里将用户的部分信息，比如 uid 存到了Claim 中，如果你想知道如何在其他地方将这个 uid从 Token 中取出来，请看下边的SerializeJwt() 方法，或者在整个解决方案，搜索这个方法，看哪里使用了！
                   2、你也可以研究下 HttpContext.User.Claims ，具体的你可以看看 Policys/PermissionHandler.cs 类中是如何使用的。
                 */
                new Claim("UserId",model.UserId.ToString()),
                new Claim("UserName",model.UserName),
                //new Claim(ClaimTypes.Role,tokenModel.Role),//为了解决一个用户多个角色(比如：Admin,System)，用下边的方法
               };

            // 可以将一个用户的多个角色全部赋予；
            // 作者：DX 提供技术支持；
            if (!string.IsNullOrWhiteSpace(model.Role))
            {
                claims.AddRange(model.Role.Split(',').Select(s => new Claim(ClaimTypes.Role, s)));
                claims.Add(new Claim("Role", model.Role));
            }

            //秘钥 (SymmetricSecurityKey 对安全性的要求，密钥的长度太短会报出异常)
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(model.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwt = new JwtSecurityToken(
                issuer: model.Issuer,
                audience: model.Audience,
                expires: DateTime.Now.AddSeconds(model.Expires),
                signingCredentials: creds,
                claims: claims
            );

            var jwtHandler = new JwtSecurityTokenHandler();
            var token = jwtHandler.WriteToken(jwt);

            return token;
        }

        /// <summary>
        /// 不检查有效性并解析
        /// </summary>
        /// <param name="jwtStr"></param>
        /// <returns></returns>
        public static TokenModelJwt SerializeJwt(string jwtStr)
        {
            //不校验，直接解析token
            var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(jwtStr);
            var tokenJwt = JsonConvert.DeserializeObject<TokenModelJwt>(jwtToken.Payload.SerializeToJson());
            return tokenJwt;
        }

    }
    /// <summary>
    /// 令牌
    /// </summary>
    public class TokenModelJwt
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Secret { get; set; }
        public int Expires { get; set; }
        public string Role { get; set; }
    }
}
