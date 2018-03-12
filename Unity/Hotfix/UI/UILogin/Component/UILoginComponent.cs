using System;
using System.Net;
using ETModel;
using UnityEngine;
using UnityEngine.UI;

namespace ETHotfix
{
	[ObjectSystem]
	public class UiLoginComponentSystem : AwakeSystem<UILoginComponent>
	{
		public override void Awake(UILoginComponent self)
		{
			self.Awake();
		}
	}
	
	public class UILoginComponent: Component
	{
		private GameObject account;
		private GameObject loginBtn;
	    private GameObject registerBtn;
	    private GameObject password;
	    private string accountText;
	    private string passwordText;
	    private SessionWrap sessionWrap = null;

        public void Awake()
		{
			ReferenceCollector rc = this.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
			loginBtn = rc.Get<GameObject>("LoginBtn");
		    this.registerBtn = rc.Get<GameObject>("RegisterBtn");
			loginBtn.GetComponent<Button>().onClick.Add(OnLogin);
		    registerBtn.GetComponent<Button>().onClick.Add(OnRegister);


			this.account = rc.Get<GameObject>("Account");
			this.password = rc.Get<GameObject>("Password");
        }

	    private async void OnRegister()
	    {
            try
	        {
	            this.accountText = this.account.GetComponent<InputField>().text;
	            this.passwordText = this.password.GetComponent<InputField>().text;
                if (String.IsNullOrWhiteSpace(accountText) || string.IsNullOrWhiteSpace(passwordText))
	            {
	                Log.Error("账号或者密码为空");
	                return;
	            }

	            IPEndPoint connetEndPoint = NetworkHelper.ToIPEndPoint(GlobalConfigComponent.Instance.GlobalProto.Address);

	            Session session = ETModel.Game.Scene.GetComponent<NetOuterComponent>().Create(connetEndPoint);
	            sessionWrap = new SessionWrap(session);
	            R2C_Register register = (R2C_Register) await sessionWrap.Call(new C2R_Register() { Account = accountText, Password = passwordText });
	            switch (register.Error)
	            {
                    case ErrorCode.ERR_Success:
                        Log.Info("成功注册");
                        break;
                    case ErrorCode.ERR_AccountExist:
                        Log.Error("用户名已被注册");
                        break;
	            }
	        }
	        catch (Exception e)
	        {
	            sessionWrap?.Dispose();
	            Log.Error(e.ToStr());
            }
        }

	    public async void OnLogin()
		{
			try
			{
			    this.accountText = this.account.GetComponent<InputField>().text;
			    this.passwordText = this.password.GetComponent<InputField>().text;

                if (String.IsNullOrWhiteSpace(accountText) || string.IsNullOrWhiteSpace(passwordText))
			    {
			        Log.Error("----账号或者密码为空");
			        return;
			    }

                //登录成功
                R2C_Login r2CLogin = (R2C_Login) await sessionWrap.Call(new C2R_Login() { Account = accountText, Password = passwordText });

			    if (r2CLogin.Error != 0)
			    {
			        Log.Error($"登录失败:{r2CLogin.Error}");
			        return;
			    }
				sessionWrap.Dispose();

                //连接gate
			    IPEndPoint connetEndPoint = NetworkHelper.ToIPEndPoint(r2CLogin.Address);
				Session gateSession = ETModel.Game.Scene.GetComponent<NetOuterComponent>().Create(connetEndPoint);
				Game.Scene.AddComponent<SessionWrapComponent>().Session = new SessionWrap(gateSession);
				ETModel.Game.Scene.AddComponent<SessionComponent>().Session = gateSession;
				G2C_LoginGate g2CLoginGate = (G2C_LoginGate)await SessionWrapComponent.Instance.Session.Call(new C2G_LoginGate() { Key = r2CLogin.Key });
				Log.Info("登陆gate成功!");

				// 创建Player
//				Player player = ETModel.ComponentFactory.CreateWithId<Player>(g2CLoginGate.PlayerId);
//				PlayerComponent playerComponent = ETModel.Game.Scene.GetComponent<PlayerComponent>();
//				playerComponent.MyPlayer = player;
//
//				Game.Scene.GetComponent<UIComponent>().Create(UIType.UILobby);
//				Game.Scene.GetComponent<UIComponent>().Remove(UIType.UILogin);
			}
			catch (Exception e)
			{
				sessionWrap?.Dispose();
				Log.Error(e.ToStr());
			}
		}
	}
}
