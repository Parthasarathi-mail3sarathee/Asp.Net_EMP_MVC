using System;
using System.Collections.Generic;
using System.Threading;
using WebApplication_Shared_Services.Model;
using static WebApplication_Session.SingletonUserSessionRepo;

namespace WebApplication_Session
{
    public interface ISessionManager
    {
        void setSession(string key, Session value);
        object getSession(string key);
    }
    public class SessionManager : ISessionManager
    {
        public SingletonUserSessionRepo CurrentSession { get; set; }

        public double SessionTimeout { get; set; }

        SessionManager()
        {
            CurrentSession = SingletonUserSessionRepo.Instance;
        }
        public void setSession(string key, Session value)
        {
            CurrentSession.session.Add(key, value);
        }
        public object getSession(string key)
        {
            Session val = null;
            CurrentSession.session.TryGetValue(key, out val);
            return val;
        }

        public void setTimeforSession(double seconds = 1800000)
        {
            var timer = new System.Threading.Timer(e => clearSession(), null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
        }

        public void clearSession()
        {

        }
    }
}


