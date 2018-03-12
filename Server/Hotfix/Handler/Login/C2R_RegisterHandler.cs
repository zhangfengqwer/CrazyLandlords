using System;
using System.Collections.Generic;
using System.Net;
using ETModel;
using MongoDB.Bson;

namespace ETHotfix
{
    [MessageHandler(AppType.Realm)]
    public class C2R_RegisterHandler : AMRpcHandler<C2R_Register, R2C_Register>
    {
        protected override async void Run(Session session, C2R_Register message, Action<R2C_Register> reply)
        {
            Log.Info($"----{JsonHelper.ToJson(message)}");
            R2C_Register register = new R2C_Register();
            try
            {
                string query = $"{"{"}'Account':'{message.Account}'{"}"}";
                Log.Info($"----{query}");

                List<AccountInfo> accounts = await Game.Scene.GetComponent<DBProxyComponent>().QueryJson<AccountInfo>(query);
                if (accounts.Count > 0)
                {
                    register.Error = ErrorCode.ERR_AccountExist;
                    reply(register);
                    return;
                }

                Log.Info($"----{accounts.Count}");
                AccountInfo accountInfo = ComponentFactory.CreateWithId<AccountInfo>(IdGenerater.GenerateId());
                accountInfo.Account = message.Account;
                accountInfo.Password = message.Password;

                await Game.Scene.GetComponent<DBProxyComponent>().Save(accountInfo);
                reply(register);
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
                ReplyError(register, e, reply);
            }
        }
    }
}