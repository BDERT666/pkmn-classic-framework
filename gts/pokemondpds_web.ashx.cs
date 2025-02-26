﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using GamestatsBase;
using PkmnFoundations.Data;
using PkmnFoundations.Structures;

namespace PkmnFoundations.GTS
{
    /// <summary>
    /// Summary description for pokemondpds_web
    /// </summary>
    public class pokemondpds_web : GamestatsHandler
    {
        public pokemondpds_web()
            : base("uLMOGEiiJogofchScpXb000244fd00006015100000005b440e7epokemondpds",
            GamestatsRequestVersions.Version3, GamestatsResponseVersions.Version2, true, true)
        {

        }

        public override void ProcessGamestatsRequest(byte[] request, MemoryStream response, string url, int pid, HttpContext context, GamestatsSession session)
        {
            switch (url)
            {
                default:
                    SessionManager.Remove(session);

                    // unrecognized page url
                    ShowError(context, 404);
                    return;

                case "/pokemondpds/web/enc/lobby/checkProfile.asp":
                {
                    if (request.Length != 168)
                    {
                        ShowError(context, 400);
                        return;
                    }

                    // I am going to guess that the PID provided second is the
                    // one whose data should appear in the response.
                    int requestedPid = BitConverter.ToInt32(request, 0);
                    byte[] requestDataPrefix = new byte[12];
                    byte[] requestData = new byte[152];

                    Array.Copy(request, 4, requestDataPrefix, 0, 12);
                    Array.Copy(request, 16, requestData, 0, 152);

                    TrainerProfilePlaza requestProfile = new TrainerProfilePlaza(pid, requestDataPrefix, requestData);
                    Database.Instance.PlazaSetProfile(requestProfile);

                    TrainerProfilePlaza responseProfile = Database.Instance.PlazaGetProfile(requestedPid);
                    response.Write(responseProfile.Data, 0, 152);

                } break;

                case "/pokemondpds/web/enc/lobby/getSchedule.asp":
                {
                    // This is a replayed response from a game I had with Pipian.
                    // It appears to be 49 ints.
                    // todo: A real implementation

                    response.Write(new byte[]
                    {
                        0x00, 0x00, 0x00, 0x00, 0xb0, 0x04, 0x00, 0x00, 
                        0x9e, 0xc4, 0x70, 0xa7, 0x00, 0x00, 0x00, 0x00, 
                        0x03, 0x00, 0x16, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x0b, 0x00, 0x00, 0x00, 0x0c, 0x03, 0x00, 0x00, 
                        0x08, 0x00, 0x00, 0x00, 0x48, 0x03, 0x00, 0x00, 
                        0x02, 0x00, 0x00, 0x00, 0x48, 0x03, 0x00, 0x00, 
                        0x09, 0x00, 0x00, 0x00, 0x84, 0x03, 0x00, 0x00, 
                        0x03, 0x00, 0x00, 0x00, 0x84, 0x03, 0x00, 0x00, 
                        0x0a, 0x00, 0x00, 0x00, 0x84, 0x03, 0x00, 0x00, 
                        0x0c, 0x00, 0x00, 0x00, 0xc0, 0x03, 0x00, 0x00, 
                        0x04, 0x00, 0x00, 0x00, 0xc0, 0x03, 0x00, 0x00, 
                        0x09, 0x00, 0x00, 0x00, 0xc0, 0x03, 0x00, 0x00, 
                        0x0d, 0x00, 0x00, 0x00, 0xc0, 0x03, 0x00, 0x00, 
                        0x0f, 0x00, 0x00, 0x00, 0xfc, 0x03, 0x00, 0x00, 
                        0x05, 0x00, 0x00, 0x00, 0xfc, 0x03, 0x00, 0x00, 
                        0x0e, 0x00, 0x00, 0x00, 0xfc, 0x03, 0x00, 0x00, 
                        0x10, 0x00, 0x00, 0x00, 0x33, 0x04, 0x00, 0x00, 
                        0x12, 0x00, 0x00, 0x00, 0x38, 0x04, 0x00, 0x00, 
                        0x06, 0x00, 0x00, 0x00, 0x38, 0x04, 0x00, 0x00, 
                        0x0d, 0x00, 0x00, 0x00, 0x38, 0x04, 0x00, 0x00, 
                        0x11, 0x00, 0x00, 0x00, 0x74, 0x04, 0x00, 0x00, 
                        0x0b, 0x00, 0x00, 0x00, 0xb0, 0x04, 0x00, 0x00, 
                        0x13, 0x00, 0x00, 0x00
                    }, 0, 196);

                } break;

                case "/pokemondpds/web/enc/lobby/getVIP.asp":
                {
                    response.Write(new byte[] { 0x00, 0x00, 0x00, 0x00 }, 0, 4);

                        foreach (var i in new[] { 600403373, 601315647 })
                        {
                            response.Write(BitConverter.GetBytes(i), 0, 4);
                            response.Write(new byte[] { 0x00, 0x00, 0x00, 0x00 }, 0, 4);
                        }

                    }
                break;

                case "/pokemondpds/web/enc/lobby/getQuestionnaire.asp":
                {
                    response.Write(new byte[]{

                        0x00, 0x00, 0x00, 0x00, 0x2a, 0x01, 0x00, 
                        0x00, 0x2d, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x01, 
                        0x01, 0x01, 0x01, 0x00, 0x01, 0x01, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x29, 0x01, 0x00, 
                        0x00, 0x2c, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x01, 
                        0x01, 0x01, 0x01, 0x00, 0x01, 0x01, 0x00, 0x00, 
                        0x00, 0x00, 0x00, 0x00, 0x00, 0x7e, 0x00, 0x00, 
                        0x00, 0x46, 0x00, 0x00, 0x00, 0x33, 0x00, 0x00, 
                        0x00, 0x64, 0x01, 0x00, 0x00, 0x11, 0x01, 0x00, 
                        0x00, 0x83, 0x00, 0x00, 0x00
                    }, 0, 732);

                } break;

                case "/pokemondpds/web/enc/lobby/submitQuestionnaire.asp":
                {
                    // literally 'thx' in ascii... lol
                    response.Write(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x74, 0x68, 0x78, 0x00 }, 0, 8);

                } break;
            }
        }
    }
}
