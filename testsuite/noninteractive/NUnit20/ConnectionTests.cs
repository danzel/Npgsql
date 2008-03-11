// project created on 30/11/2002 at 22:00
//
// Author:
// 	Francisco Figueiredo Jr. <fxjrlists@yahoo.com>
//
//	Copyright (C) 2002 The Npgsql Development Team
//	npgsql-general@gborg.postgresql.org
//	http://gborg.postgresql.org/project/npgsql/projdisplay.php
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA

using System;
using Npgsql;
using System.Data;
using System.Resources;
using NUnit.Framework;

namespace NpgsqlTests
{

    [TestFixture]
    public class ConnectionTests : BaseClassTests
    {
        
        [Test]
        public void ChangeDatabase()
        {
            _conn.ChangeDatabase("template1");

            NpgsqlCommand command = new NpgsqlCommand("select current_database()", _conn);

            String result = (String)command.ExecuteScalar();

            Assert.AreEqual("template1", result);
        }

		[Test]
		public void ChangeDatabaseTestConnectionCache()
		{
			NpgsqlConnection conn1 = new NpgsqlConnection(_connString);
			NpgsqlConnection conn2 = new NpgsqlConnection(_connString);

			NpgsqlCommand command;

			//	connection 1 change database
			conn1.Open();
			conn1.ChangeDatabase("template1");
			command = new NpgsqlCommand("select current_database()", conn1);
			string db1 = (String)command.ExecuteScalar();

			Assert.AreEqual("template1", db1);

			//	connection 2 's database should not changed, so should different from conn1
			conn2.Open();
			command = new NpgsqlCommand("select current_database()", conn2);
			string db2 = (String)command.ExecuteScalar();

			Assert.AreNotEqual(db1, db2);

			conn1.Close();
			conn2.Close();
		}

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void NestedTransaction()
        {
            NpgsqlTransaction t;
                
            t = _conn.BeginTransaction();
            if(t == null)
              return;
        }

        [Test]
        public void SequencialTransaction()
        {
            _t.Rollback();

            _t = _conn.BeginTransaction();
        }

        [Test]
        public void ConnectionRefused()
        {
            try
            {
                NpgsqlConnection conn = new NpgsqlConnection("Server=127.0.0.1;Port=44444;User Id=npgsql_tets;Password=j");

                conn.Open();
            }

            catch (NpgsqlException e)
            {
				Type type_NpgsqlState = typeof(NpgsqlConnection).Assembly.GetType("Npgsql.NpgsqlState");
				ResourceManager resman = new ResourceManager(type_NpgsqlState);
				string expected = string.Format(resman.GetString("Exception_FailedConnection"), "127.0.0.1");
				Assert.AreEqual(expected, e.Message);
            }
        }
		
		[Test]
        [ExpectedException(typeof(NpgsqlException))]
        public void ConnectionStringWithEqualSignValue()
        {
            
            NpgsqlConnection conn = new NpgsqlConnection("Server=127.0.0.1;Port=44444;User Id=npgsql_tets;Password=j==");
            
            conn.Open();
            
        }
        
        [Test]
        [ExpectedException(typeof(NpgsqlException))]
        public void ConnectionStringWithSemicolonSignValue()
        {
            
            NpgsqlConnection conn = new NpgsqlConnection("Server=127.0.0.1;Port=44444;User Id=npgsql_tets;Password='j;'");
            
            conn.Open();
            
        }
        
        [Test]
        public void SearchPathSupport()
        {
            
            NpgsqlConnection conn = new NpgsqlConnection(_connString + ";searchpath=public");
            conn.Open();
            
            NpgsqlCommand c = new NpgsqlCommand("show search_path", conn);
            
            String searchpath = (String) c.ExecuteScalar();
            //Note, public is no longer implicitly added to paths, so this is no longer "public, public".
            Assert.AreEqual("public", searchpath );
            
            
        }
		
    }
    [TestFixture]
    public class ConnectionTestsV2 : BaseClassTests
    {
        
        [Test]
        public void ChangeDatabase()
        {
            _connV2.ChangeDatabase("template1");

            NpgsqlCommand command = new NpgsqlCommand("select current_database()", _connV2);

            String result = (String)command.ExecuteScalar();

            Assert.AreEqual("template1", result);
        }

		[Test]
		public void ChangeDatabaseTestConnectionCache()
		{
			NpgsqlConnection conn1 = new NpgsqlConnection(_connV2String);
			NpgsqlConnection conn2 = new NpgsqlConnection(_connV2String);

			NpgsqlCommand command;

			//	connection 1 change database
			conn1.Open();
			conn1.ChangeDatabase("template1");
			command = new NpgsqlCommand("select current_database()", conn1);
			string db1 = (String)command.ExecuteScalar();

			Assert.AreEqual("template1", db1);

			//	connection 2 's database should not changed, so should different from conn1
			conn2.Open();
			command = new NpgsqlCommand("select current_database()", conn2);
			string db2 = (String)command.ExecuteScalar();

			Assert.AreNotEqual(db1, db2);

			conn1.Close();
			conn2.Close();
		}

        [Test]
        [ExpectedException(typeof(InvalidOperationException))]
        public void NestedTransaction()
        {
            NpgsqlTransaction t;
                
            t = _connV2.BeginTransaction();
            if(t == null)
              return;
        }

        [Test]
        public void SequencialTransaction()
        {
            _tV2.Rollback();

            _tV2 = _connV2.BeginTransaction();
        }

        [Test]
        public void ConnectionRefused()
        {
            try
            {
                NpgsqlConnection conn = new NpgsqlConnection("Server=127.0.0.1;Port=44444;User Id=npgsql_tets;Password=j");

                conn.Open();
            }

            catch (NpgsqlException e)
            {
				Type type_NpgsqlState = typeof(NpgsqlConnection).Assembly.GetType("Npgsql.NpgsqlState");
				ResourceManager resman = new ResourceManager(type_NpgsqlState);
				string expected = string.Format(resman.GetString("Exception_FailedConnection"), "127.0.0.1");
				Assert.AreEqual(expected, e.Message);
            }
        }
		
		[Test]
        [ExpectedException(typeof(NpgsqlException))]
        public void ConnectionStringWithEqualSignValue()
        {
            
            NpgsqlConnection conn = new NpgsqlConnection("Server=127.0.0.1;Port=44444;User Id=npgsql_tets;Password=j==");
            
            conn.Open();
            
        }
        
        [Test]
        [ExpectedException(typeof(NpgsqlException))]
        public void ConnectionStringWithSemicolonSignValue()
        {
            
            NpgsqlConnection conn = new NpgsqlConnection("Server=127.0.0.1;Port=44444;User Id=npgsql_tets;Password='j;'");
            
            conn.Open();
            
        }
        
        [Test]
        public void SearchPathSupport()
        {
            
            NpgsqlConnection conn = new NpgsqlConnection(_connV2String + ";searchpath=public");
            conn.Open();
            
            NpgsqlCommand c = new NpgsqlCommand("show search_path", conn);
            
            String searchpath = (String) c.ExecuteScalar();
            //Note, public is no longer implicitly added to paths, so this is no longer "public, public".
            Assert.AreEqual("public", searchpath );
            
            
        }
		
    }
}
