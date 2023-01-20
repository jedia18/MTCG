﻿using Microsoft.VisualBasic;
using MTCG.Controller;
using Newtonsoft.Json.Linq;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;



namespace MTCG
{
    public static class Program
    {  
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // main entry point                                                                                         //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Entry point.</summary>
        /// <param name="args">Command line arguments.</param>
        static void Main(string[] args)
        {
            //InitDb();
            IncomingHandler.InitDb(); 

            HttpSvr svr = new HttpSvr();
            svr.Incoming += _Svr_Incoming;

            svr.Run();
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // event handlers                                                                                           //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Processes an incoming HTTP request.</summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">Event arguments.</param>
        public static void _Svr_Incoming(object sender, HttpSvrEventArgs e)
        {
            ThreadPool.QueueUserWorkItem(IncomingHandler._Svr_Incoming, e);
        }      
        
    }
}
