using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace XMLtoSQLExport
{
    class Program
    {
        static XmlDocument xmlDoc;
        static StringBuilder sb;
        static String organizationID = "29005";
        static String teamID = "202397";
        static String xmlPath = "D:/Desktop/XmlToSQLExporter/XMLtoSQLExport/XMLtoSQLExport/XMLFiles/";
        static String resultPath = "D:/Desktop/XmlToSQLExporter/XMLtoSQLExport/XMLtoSQLExport/Results/";
        static String SQLQuery;
        static String firstName;
        static String lastname;
        static String email;
        static String password;
        static String result = "";

        static void Main(string[] args){
            
            xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlPath + "TestExport.xml");
            sb = new StringBuilder("");

            foreach(XmlNode node in xmlDoc.DocumentElement.ChildNodes){

                if (node.SelectSingleNode("first_name") != null){ firstName = node.SelectSingleNode("first_name").InnerText; }

                if (node.SelectSingleNode("last_name") != null){ lastname = node.SelectSingleNode("last_name").InnerText; }

                if (node.SelectSingleNode("email") != null){ email = node.SelectSingleNode("email").InnerText; }

                if (node.SelectSingleNode("password") != null){ password = node.SelectSingleNode("password").InnerText; }else{ break; }

                SQLQuery = 
                "SET @usermail = '" + email + "';\n" +
                "SET @userPassword = '" + password + "';\n" +
                "SET @username = '" + firstName + "';\n" +
                "SET @userLastName = '" + lastname + "';\n" +
                "SET @userOrganization = " + organizationID + ";\n" +
                "SET @userTeam = " + teamID + ";\n" +
                "INSERT INTO lucidious_users(name, pass, mail, status, language, init, picture) VALUES (@userMail, @userPassword, @userMail, 1, 'nl', @userMail, 0);\n" +
                "SET @userID = LAST_INSERT_ID();\n" +
                "INSERT INTO test_field_revision_field_first_name (entity_type, bundle, deleted, entity_id, revision_id, language, delta, field_first_name_value, field_first_name_format) VALUES ('user', 'user', 0, @userID, @userID, 'und', 0, @userName, null);\n" +
                "INSERT INTO test_field_revision_field_last_name (entity_type, bundle, deleted, entity_id, revision_id, language, delta, field_last_name_value, field_last_name_format) VALUES ('user', 'user', 0, @userID, @userID, 'und', 0, @userLastname, null);\n" +
                "INSERT INTO test_field_revision_field_organization (entity_type, bundle, deleted, entity_id, language, delta, field_organization_target_id) VALUES ('user', 'user', 0, @userID, 'und', 0, @userOrganization);\n" +
                "INSERT INTO test_field_revision_field_team (entity_type, bundle, deleted, entity_id, language, delta, field_team_target_id) VALUES ('user', 'user', 0, @userID, 'und', 0, @userTeam);\n" +
                "INSERT INTO test_field_data_field_first_name (entity_type, bundle, deleted, entity_id, revision_id, language, delta, field_first_name_value, field_first_name_format) VALUES ('user', 'user', 0, @userID, @userID, 'und', 0, @userName, null);\n" +
                "INSERT INTO test_field_data_field_last_name (entity_type, bundle, deleted, entity_id, revision_id, language, delta, field_last_name_value, field_last_name_format) VALUES ('user', 'user', 0, @userID, @userID, 'und', 0, @userLastname, null);\n" +
                "INSERT INTO test_field_data_field_organization (entity_type, bundle, deleted, entity_id, language, delta, field_organization_target_id) VALUES ('user', 'user', 0, @userID, 'und', 0, @userOrganization);\n" +
                "INSERT INTO test_field_data_field_team (entity_type, bundle, deleted, entity_id, language, delta, field_team_target_id) VALUES ('user', 'user', 0, @userID, 'und', 0, @userTeam);\n";

                sb.Append(SQLQuery);
            }
            
            result = sb.ToString();

            Console.WriteLine(result);

            System.IO.File.WriteAllText(@resultPath + "UserExportSQL.txt", result);

             Console.WriteLine(" ===== SQL Creation complete! ===== ");

            Console.ReadLine();
        }
    }
}
