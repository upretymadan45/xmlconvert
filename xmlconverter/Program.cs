using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace xmlconverter
{
    class Program
    {
        static void Main(string[] args)
        {
            LoadMenu();
        }

        private static void LoadMenu()
        {
            int opt;
            do
            {
                Console.WriteLine();
                Console.WriteLine("Welcome to xml file converter");
                Console.WriteLine("====================================");
                Console.WriteLine("Please select your option");
                Console.WriteLine("1. Load XML data into database");
                Console.WriteLine("2. Process XML data");
                Console.WriteLine("3. Quit");
                Console.WriteLine("====================================");
                Console.WriteLine("Enter your option:");
                Console.WriteLine();

                var optionInput = Console.ReadLine();
                if (string.IsNullOrEmpty(optionInput))
                    return;
                else
                    opt = Convert.ToInt32(optionInput);

                switch (opt)
                {
                    case 1:
                        LoadXmlToDatabase();
                        break;

                    case 2:
                        RemoveDuplicatesFromPerson();
                        RemoveDuplicatesFromRegistration();
                        CountPersonAndRegistrationTableRows();
                        ComparePersonTables();
                        CompareRegistrationTables();
                        CompareAddressTables();
                        CompareLanguageTables();
                        CompareCautionTables();
                        CompareConditionTables();
                        CompareEndorsementTables();
                        CompareNotationTables();
                        CompareQualificationTables();
                        CompareReprimandTables();
                        CompareRestrictionTables();
                        CompareSpecialtyTables();
                        CompareUndertakingTables();
                        Console.WriteLine("Comparison results are stored in excel file in bin/debug/comparison/");

                        break;

                    case 3:
                        Console.WriteLine("Press any key to continue..");
                        Console.ReadLine();
                        break;

                    default:
                        Console.WriteLine("Invalid option");
                        Console.ReadLine();
                        break;
                }
            } while (opt >= 1 && opt <= 2);
        }

        static void LoadXmlToDatabase()
        {
            try
            {
                Console.WriteLine("Please enter the xml file location(Default: C:\\sample_pph.xml)");
                Console.WriteLine("Press enter if default location or input file path");
                string path = Console.ReadLine();
                if (string.IsNullOrEmpty(path))
                {
                    path = @"C:\sample_pph.xml";
                }

                if (!File.Exists(path))
                {
                    Console.WriteLine("File does not exists. Please try again..");
                    Console.WriteLine();
                    return;
                }

                var connString = GetConnectionString();
                using (var conn = new SqlConnection(connString))
                {
                    conn.Open();

                    var sql = "sp_readXmlFile";

                    var command = new SqlCommand(sql, conn);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@fileName", path);

                    var rowsAffected = command.ExecuteNonQuery();
                }
                Console.WriteLine("XML data successfully loaded into database");
                Console.WriteLine();
            }
            catch (Exception)
            {
                Console.WriteLine("Tables already exists in database. Please remove them first..");
            }
        }

        static void ComparePersonTables()
        {
            DataTable personTable = new DataTable();

            var tableName = "Person";

            var fields = new List<string> 
            { 
                "ContactNumber","GivenName","FamilyName","MiddleName","Title","NameFlag","Sex","EmailAddress","LastEditDate"
            };

           CompareTable(personTable, tableName, fields);
        }

        static void CompareRegistrationTables()
        {
            DataTable registrationTable = new DataTable();

            var tableName = "Registration";

            var fields = new List<string>
            {
                "ProfessionNumber","Profession","RegistrationType",
                "RegistrationSubType","RegistrationStatus","RegistrationSubStatus",
                "SubStatusChangeDate","SuppressFlag","SuppressDate","SuppressReason",
                "RegistrationStartDate","RenewalDate","LastEditDate","PersonId"
            };

            CompareTable(registrationTable, tableName, fields);
        }

        static void CompareAddressTables()
        {
            DataTable addressTable = new DataTable();

            var tableName = "Address";

            var fields = new List<string>
            {
                "AddressType","CountryName","State",
                "Suburb","Postcode","AddressEditDate",
                "PersonId"
            };

            CompareTable(addressTable, tableName, fields);
        }

        static void CompareLanguageTables()
        {
            DataTable languageTable = new DataTable();

            var tableName = "Language";

            var fields = new List<string>
            {
                "Language","PersonId"
            };

            CompareTable(languageTable, tableName, fields);
        }

        static void CompareCautionTables()
        {
            DataTable cautionTable = new DataTable();

            var tableName = "Caution";

            var fields = new List<string>
            {
                "CautionType","CautionText","CautionStatus","CautionStartDate","CautionEndDate","CautionEditDate","RegistrationId"
            };

            CompareTable(cautionTable, tableName, fields);
        }

        static void CompareConditionTables()
        {
            DataTable conditionTable = new DataTable();

            var tableName = "Condition";

            var fields = new List<string>
            {
                "ConditionType","ConditionText","ConditionStatus","ConditionStartDate","ConditionEndDate","ConditionEditDate","RegistrationId"
            };

            CompareTable(conditionTable, tableName, fields);
        }

        static void CompareEndorsementTables()
        {
            DataTable endorsementTable = new DataTable();

            var tableName = "Endorsement";

            var fields = new List<string>
            {
                "EndorsementType","EndorsementSubType","EndorsementText","EndorsementStartDate","EndorsementEndDate","EndorsementEditDate","RegistrationId"
            };

            CompareTable(endorsementTable, tableName, fields);
        }

        static void CompareNotationTables()
        {
            DataTable notationTable = new DataTable();

            var tableName = "Notation";

            var fields = new List<string>
            {
                "NotationType","NotationText","NotationStatus","NotationOnRegisterFlag","NotationEndDate","NotationEditDate","RegistrationId"
            };

            CompareTable(notationTable, tableName, fields);
        }

        static void CompareQualificationTables()
        {
            DataTable qualificationTable = new DataTable();

            var tableName = "Qualification";

            var fields = new List<string>
            {
                "QualificationName","AwardYear","AwardingAuthority","CountryName","LastEditDate","PersonId"
            };

            CompareTable(qualificationTable, tableName, fields);
        }

        static void CompareReprimandTables()
        {
            DataTable reprimandTable = new DataTable();

            var tableName = "Reprimand";

            var fields = new List<string>
            {
                "ReprimandType","ReprimandText","ReprimandStatus","ReprimandStartDate","ReprimandEndDate","ReprimandEditDate","RegistrationId"
            };

            CompareTable(reprimandTable, tableName, fields);
        }

        static void CompareRestrictionTables()
        {
            DataTable restriction = new DataTable();

            var tableName = "Restriction";

            var fields = new List<string>
            {
                "RestrictionType","RestrictionText","RestrictionStatus","RestrictionStartDate","RestrictionEndDate","RestrictionEditDate","RegistrationId"
            };

            CompareTable(restriction, tableName, fields);
        }

        static void CompareSpecialtyTables()
        {
            DataTable specialtyTable = new DataTable();

            var tableName = "Specialty";

            var fields = new List<string>
            {
                "SpecialtyField","SpecialtySubType","RegistrationId"
            };

            CompareTable(specialtyTable, tableName, fields);
        }

        static void CompareUndertakingTables()
        {
            DataTable undertakingTable = new DataTable();

            var tableName = "Undertaking";

            var fields = new List<string>
            {
                "UndertakingType","UndertakingText","UndertakingStatus","UndertakingStartDate","UndertakingEndDate","UndertakingEditDate","RegistrationId"
            };

            CompareTable(undertakingTable, tableName, fields);
        }

        private static void CompareTable(DataTable cautionTable, string tableName, List<string> fields)
        {
            //outputs csv file in ProjectDirectory/Bin/Debug folder
            var csvFilePath = Path.Combine(Directory.GetCurrentDirectory(), "comparison",tableName+"_compare_result.csv");
            if (File.Exists(csvFilePath))
            {
                File.Delete(csvFilePath);
            }

            //csv heading..
            using (var stream = File.AppendText(csvFilePath))
            {
                string csvHeading = string.Format("{0},{1},{2},{3}", tableName + " Table", "Field Name", "Expected Value", "Actual Value");
                stream.WriteLine(csvHeading);
            }

            using (var conn = new SqlConnection(GetConnectionString()))
            {
                conn.Open();

                var sql = "sp_findDifferences";

                var command = new SqlCommand(sql, conn);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@tableName", tableName);

                var adapter = new SqlDataAdapter(command);

                adapter.Fill(cautionTable);
            }

            GenericTableComparisonCsvOutput(cautionTable,
                                csvFilePath,
                                "pk_" + tableName,
                                fields,
                                "In DB_XML_DATA " + tableName + " table but not in DB_DATA_SET " + tableName + " table",
                                tableName);
        }

        static void CountPersonAndRegistrationTableRows()
        {
            int personRows, registrationRows;

            using(var conn = new SqlConnection(GetConnectionString()))
            {
                conn.Open();

                var personCountQuery = "select count(*) from Person";
                var registrationCountQuery = "select count(*) from Registration";

                var personCommand = new SqlCommand(personCountQuery, conn);
                var registrationCommand = new SqlCommand(registrationCountQuery, conn);

                personRows = Convert.ToInt32(personCommand.ExecuteScalar());
                registrationRows = Convert.ToInt32(registrationCommand.ExecuteScalar());
            }

            Console.WriteLine("Person table has {0} records and Registration table has {1} records", personRows,registrationRows);
        }

        static void RemoveDuplicatesFromPerson()
        {
            var query = @"WITH cte AS (
                            SELECT
                                pk_person,
                                ContactNumber,
                                GivenName,
                                FamilyName,
                                MiddleName,
                                Title,
                                NameFlag,
                                Sex,
                                EmailAddress,
                                LastEditDate,
                                ROW_NUMBER() OVER(
                                    PARTITION BY
                                        ContactNumber,
                                        GivenName,
                                        FamilyName,
                                        MiddleName,
                                        Title,
                                        NameFlag,
                                        Sex,
                                        EmailAddress,
                                        LastEditDate
                                    ORDER BY
                                        ContactNumber,
                                        GivenName,
                                        FamilyName,
                                        MiddleName,
                                        Title,
                                        NameFlag,
                                        Sex,
                                        EmailAddress,
                                        LastEditDate
                                ) row_num
                             FROM Person
                        )
                        DELETE FROM cte
                        WHERE row_num > 1";

            using(var conn = new SqlConnection(GetConnectionString()))
            {
                conn.Open();
                var command = new SqlCommand(query, conn);
                command.ExecuteNonQuery();
            }

            Console.WriteLine("Duplicate records from person table removed");
        }

        static void RemoveDuplicatesFromRegistration()
        {
            var query = @"WITH cte AS (
                                SELECT
                                    pk_registration,
                                    ProfessionNumber,
                                    Profession,
                                    Division,
                                    RegistrationType,
                                    RegistrationSubType,
                                    RegistrationStatus,
                                    RegistrationSubStatus,
                                    SubStatusChangeDate,
                                    SuppressFlag,
                                    SuppressDate,
                                    SuppressReason,
                                    RegistrationStartDate,
                                    RenewalDate,
                                    LastEditDate,
                                    PersonId,
                                    ROW_NUMBER() OVER(
                                        PARTITION BY
                                            ProfessionNumber,
                                            Profession
                                        ORDER BY
                                            ProfessionNumber,
                                            Profession
                                    ) row_num
                                 FROM
                                    Registration
                            )
                            DELETE FROM cte
                            WHERE row_num > 1;";

            using(var conn = new SqlConnection(GetConnectionString()))
            {
                conn.Open();
                var command = new SqlCommand(query, conn);
                command.ExecuteNonQuery();
            }

            Console.WriteLine("Duplicate records from Registration table removed");
        }

        private static void PersonComparisonCsvOutput(DataTable personTable, string csvFilePath)
        {
            if (personTable.Rows.Count == 0)
            {
                using (var stream = File.AppendText(csvFilePath))
                {
                    string csvRow = string.Format("{0}","Records from both database table matches with no error(s)");
                    stream.WriteLine(csvRow);
                }
            }
            else
            {
                var missingRecords = new List<MissingRecord>();
                var usedPks = new List<decimal>();

                foreach (DataRow row in personTable.Rows)
                {
                    var pkPerson = Convert.ToDecimal(row["pk_person"]);

                    if (usedPks.Contains(pkPerson))
                        continue;

                    DataRow[] duplicateRows = personTable.Select("pk_person=" + pkPerson);
                    if (duplicateRows.Length > 1)
                    {
                        var dbName1 = duplicateRows[0]["unmatched_records_src"].ToString();
                        var dbName2 = duplicateRows[1]["unmatched_records_src"].ToString();

                        string actualName = "", expectedName = "";
                        using (var stream = File.AppendText(csvFilePath))
                        {
                            if (dbName1.Contains("In DB_XML_DATA Person table but not in DB_DATA_SET Person table"))
                            {
                                actualName = duplicateRows[0]["GivenName"].ToString();
                                expectedName = duplicateRows[1]["GivenName"].ToString();
                            }
                            else
                            {
                                actualName = duplicateRows[1]["GivenName"].ToString();
                                expectedName = duplicateRows[0]["GivenName"].ToString();
                            }

                            string csvRow = string.Format("{0},{1}", "Actual " + actualName, "Expected " + expectedName);

                            stream.WriteLine(csvRow);
                        }

                    }
                    else
                    {
                        var dbName = duplicateRows[0]["unmatched_records_src"].ToString();
                        var id = Convert.ToDecimal(duplicateRows[0][1]);

                        missingRecords.Add(new MissingRecord { Id = id, SourceDb = dbName });
                    }

                    usedPks.Add(pkPerson);
                }

                foreach (var missingRecord in missingRecords)
                {
                    using (var stream = File.AppendText(csvFilePath))
                    {
                        string dbName = string.Empty;
                        if (missingRecord.SourceDb.Equals("In DB_XML_DATA Person table but not in DB_DATA_SET Person table"))
                        {
                            dbName = "DB_DATA_SET Person table";
                        }
                        else
                        {
                            dbName = "DB_XML_DATA Person table";
                        }

                        string csvRow = string.Format("{0}", "Record with pk_Id= " + missingRecord.Id + " Is Missing from " + dbName);
                        stream.WriteLine(csvRow);
                    }
                }
            }
        }

        private static string GetConnectionString()
        {
            var connString = ConfigurationManager.ConnectionStrings["xmlconnstring"];
            return connString.ToString();
        }

        private static void GenericTableComparisonCsvOutput(DataTable dataTable, 
                        string csvFilePath,
                        string keyField,
                        List<string> fields,
                        string xmlDatabaseTableRecordSrc,
                        string tableName)
        {
            if (dataTable.Rows.Count == 0)
            {
                using (var stream = File.AppendText(csvFilePath))
                {
                    string csvRow = string.Format("{0}", "Records from both database table matches with no error(s)");
                    stream.WriteLine(csvRow);
                }
            }
            else
            {
                var missingRecords = new List<MissingRecord>();
                var usedPks = new List<decimal>();

                foreach (DataRow row in dataTable.Rows)
                {
                    var pkKeyField = Convert.ToDecimal(row[keyField]);

                    if (usedPks.Contains(pkKeyField))
                        continue;

                    string query = string.Format("{0}='{1}'", keyField,pkKeyField);

                    DataRow[] duplicateRows = dataTable.Select(query);
                    if (duplicateRows.Length > 1)
                    {
                        var compareResults = new List<CompareResult>();

                        var dbName1 = duplicateRows[0]["unmatched_records_src"].ToString();
                        var dbName2 = duplicateRows[1]["unmatched_records_src"].ToString();

                       
                        using (var stream = File.AppendText(csvFilePath))
                        {
                            if (dbName1.Contains(xmlDatabaseTableRecordSrc))
                            {
                                foreach (var field in fields)
                                {
                                    var compareResult = new CompareResult();
                                    compareResult.FieldName = field;
                                    compareResult.ActualValue = duplicateRows[0][field].ToString();
                                    compareResult.ExpectedValue = duplicateRows[1][field].ToString();

                                    compareResults.Add(compareResult);
                                }
                            }
                            else
                            {
                                foreach (var field in fields)
                                {
                                    var compareResult = new CompareResult();
                                    compareResult.FieldName = field;
                                    compareResult.ActualValue = duplicateRows[1][field].ToString();
                                    compareResult.ExpectedValue = duplicateRows[0][field].ToString();

                                    compareResults.Add(compareResult);
                                }
                            }

                            foreach (var compareResult in compareResults)
                            {
                                string csvResultRow = string.Format("{0},{1},{2},{3}", "-", compareResult.FieldName, compareResult.ExpectedValue, compareResult.ActualValue);

                                stream.WriteLine(csvResultRow);
                            }

                            string emptyCsvRow = string.Format("{0},{1},{2},{3}","","","","");
                            stream.WriteLine(emptyCsvRow);
                        }

                    }
                    else
                    {
                        var dbName = duplicateRows[0]["unmatched_records_src"].ToString();
                        var id = Convert.ToDecimal(duplicateRows[0][1]);

                        missingRecords.Add(new MissingRecord { Id = id, SourceDb = dbName });
                    }

                    usedPks.Add(pkKeyField);
                }

                foreach (var missingRecord in missingRecords)
                {
                    using (var stream = File.AppendText(csvFilePath))
                    {
                        string dbName = string.Empty;
                        if (missingRecord.SourceDb.Equals(xmlDatabaseTableRecordSrc))
                        {
                            dbName = "DB_DATA_SET "+ tableName +" table";
                        }
                        else
                        {
                            dbName = "DB_XML_DATA "+tableName+" table";
                        }

                        string csvRow = string.Format("{0}", "Record with pk_Id= " + missingRecord.Id + " Is Missing from " + dbName);
                        stream.WriteLine(csvRow);
                    }
                }
            }
        }
    }
}
