using System;
using System.Collections.Generic;
using WebApplication_Shared_Services.Model;

namespace WebApplication_Session
{
    public sealed class SingletonUserSessionRepo
    {
        public class Session
        {
            public object SessionObject { get; set; }
            public DateTime StoredDateTime { get; set; }
        }
        private static SingletonUserSessionRepo instance = null;
        private static readonly object padlock = new object();

        public Dictionary<string, Session> session { get; set; }


        SingletonUserSessionRepo()
        {
            session = new Dictionary<string, Session>();
        }

        public static SingletonUserSessionRepo Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new SingletonUserSessionRepo();
                    }
                    return instance;
                }
            }
        }
    }
}
