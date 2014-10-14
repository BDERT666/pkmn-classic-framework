﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PkmnFoundations.Data;

namespace PkmnFoundations.GTS
{
    /// <summary>
    /// Contains collections of GTS sessions (both generations)
    /// and some indexes for fast lookup
    /// </summary>
    public class GtsSessionManager
    {
        public readonly Dictionary<String, GtsSession4> Sessions4;
        public readonly Dictionary<String, GtsSession5> Sessions5;

        public GtsSessionManager()
        {
            Sessions4 = new Dictionary<String, GtsSession4>();
            Sessions5 = new Dictionary<String, GtsSession5>();
            RefreshStats();
        }

        public void PruneSessions()
        {
            PruneSessions4();
            PruneSessions5();
        }

        public void PruneSessions4()
        {
            Dictionary<String, GtsSession4> sessions = Sessions4;
            DateTime now = DateTime.UtcNow;

            lock (sessions)
            {
                Queue<String> toRemove = new Queue<String>();
                foreach (KeyValuePair<String, GtsSession4> session in sessions)
                {
                    if (session.Value.ExpiryDate < now) toRemove.Enqueue(session.Key);
                }
                while (toRemove.Count > 0)
                {
                    sessions.Remove(toRemove.Dequeue());
                }
            }
        }

        public void PruneSessions5()
        {
            Dictionary<String, GtsSession5> sessions = Sessions5;
            DateTime now = DateTime.UtcNow;

            lock (sessions)
            {
                Queue<String> toRemove = new Queue<String>();
                foreach (KeyValuePair<String, GtsSession5> session in sessions)
                {
                    if (session.Value.ExpiryDate < now) toRemove.Enqueue(session.Key);
                }
                while (toRemove.Count > 0)
                {
                    sessions.Remove(toRemove.Dequeue());
                }
            }
        }

        public void Add(GtsSession4 session)
        {
            Sessions4.Add(session.Hash, session);
        }

        public void Add(GtsSession5 session)
        {
            Sessions5.Add(session.Hash, session);
        }

        public void Remove(GtsSession4 session)
        {
            Sessions4.Remove(session.Hash);
        }

        public void Remove(GtsSession5 session)
        {
            Sessions5.Remove(session.Hash);
        }

        /// <summary>
        /// Retrieves the GtsSessionManager from the context's application state.
        /// </summary>
        public static GtsSessionManager FromContext(HttpContext context)
        {
            object oManager = context.Application["GtsSessionManager"];
            GtsSessionManager manager = oManager as GtsSessionManager;

            if (manager == null)
            {
                manager = new GtsSessionManager();
                context.Application.Add("GtsSessionManager", manager);
            }

            return manager;
        }

        public GtsSession4 FindSession4(int pid, String url)
        {
            // todo: keep a hash table of pids that maps them onto session objects
            GtsSession4 result = null;

            foreach (GtsSession4 sess in Sessions4.Values)
            {
                if (sess.PID == pid && sess.URL == url)
                {
                    if (result != null)
                    {
                        // todo: there's more than one matching session... delete them all.
                    }
                    return sess; // temp until I get it to cleanup old sessions
                    result = sess;
                }
            }
            return result;
        }

        public GtsSession5 FindSession5(int pid, String url)
        {
            // todo: keep a hash table of pids that maps them onto session objects
            GtsSession5 result = null;

            foreach (GtsSession5 sess in Sessions5.Values)
            {
                if (sess.PID == pid && sess.URL == url)
                {
                    if (result != null)
                    {
                        // todo: there's more than one matching session... delete them all.
                    }
                    return sess; // temp until I get it to cleanup old sessions
                    result = sess;
                }
            }
            return result;
        }

        public void RefreshStats()
        {
            AvailablePokemon4 = Database.Instance.GtsAvailablePokemon4();
            AvailablePokemon5 = Database.Instance.GtsAvailablePokemon5();
        }

        public int AvailablePokemon4
        {
            get;
            private set;
        }

        public int AvailablePokemon5
        {
            get;
            private set;
        }

    }
}