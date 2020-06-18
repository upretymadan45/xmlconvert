--stored procedure to read xml file stored in location : 'C:\sample_pph.xml'
--please create this stored procedure inside DB_XML_DATA database and store xml file in 'C:\sample_pph.xml'

CREATE Procedure sp_readXmlFile @fileName nvarchar(200)
AS
BEGIN

--drop table if exists DB_XML_DATA.dbo.Person;
--drop table if exists DB_XML_DATA.dbo.Qualification;
--drop table if exists DB_XML_DATA.dbo.Registration;
--	drop table if exists DB_XML_DATA.dbo.Restriction;
--	drop table if exists DB_XML_DATA.dbo.Caution;
--	drop table if exists DB_XML_DATA.dbo.Condition;
--	drop table if exists DB_XML_DATA.dbo.Undertaking;
--	drop table if exists DB_XML_DATA.dbo.Specialty;
--	drop table if exists DB_XML_DATA.dbo.Notation;
--	drop table if exists DB_XML_DATA.dbo.Endorsement;
--	drop table if exists DB_XML_DATA.dbo.Reprimand;
--drop table if exists DB_XML_DATA.dbo.Address;
--drop table if exists DB_XML_DATA.dbo.Language;

DECLARE @xml XML

DECLARE @SQL NVARCHAR(max)= 'SET @xml = (SELECT * FROM OPENROWSET (BULK ''' + @fileName + ''', SINGLE_CLOB) AS XmlData)'

EXEC sp_executesql @SQL, N'@xml XML OUTPUT', @xml OUTPUT;

--SELECT @x = 'P FROM OPENROWSET (BULK '''+@fileName+''', SINGLE_BLOB) AS Products(P)'

DECLARE @hdoc int

EXEC sp_xml_preparedocument @hdoc OUTPUT, @xml

SELECT *
INTO Person
FROM OPENXML (@hdoc, 'NRTPM/NRTPM_Practitioners/Person', 2)
WITH (pk_person decimal, 
	ContactNumber decimal,
	GivenName varchar(200),
	FamilyName varchar(150),
	MiddleName varchar(150),
	Title varchar(50),
	NameFlag char,
	Sex varchar(10),
	EmailAddress varchar(100),
	LastEditDate datetime)

SELECT *
INTO Qualification
FROM OPENXML (@hdoc, 'NRTPM/NRTPM_Practitioners/Person/Qualifications/Qualification', 2)
WITH (pk_qualification decimal, 
	QualificationName varchar(200),
	AwardYear varchar(10),
	AwardingAuthority varchar(200),
	CountryName varchar(150),
	LastEditDate datetime,
	PersonId decimal '../../pk_person')

SELECT *
INTO Registration
FROM OPENXML (@hdoc, 'NRTPM/NRTPM_Practitioners/Person/Registrations/Registration', 2)
WITH (pk_registration decimal, 
	ProfessionNumber varchar(300),
	Profession varchar(200),
	Division varchar(200),
	RegistrationType varchar(150),
	RegistrationSubType varchar(100),
	RegistrationStatus varchar(100),
	RegistrationSubStatus varchar(100),
	SubStatusChangeDate datetime,
	SuppressFlag varchar(50),
	SuppressDate datetime,
	SuppressReason varchar(200),
	RegistrationStartDate datetime,
	RenewalDate datetime,
	LastEditDate datetime,
	PersonId decimal '../../pk_person')


SELECT *
INTO Restriction
FROM OPENXML (@hdoc, 'NRTPM/NRTPM_Practitioners/Person/Registrations/Registration/Restrictions/Restriction', 2)
WITH (pk_restriction decimal, 
	RestrictionType varchar(100),
	RestrictionText varchar(500),
	RestrictionStatus varchar(100),
	RestrictionStartDate datetime,
	RestrictionEndDate datetime,
	RestrictionEditDate datetime,
	RegistrationId decimal '../../pk_registration')


SELECT *
INTO Caution
FROM OPENXML (@hdoc, 'NRTPM/NRTPM_Practitioners/Person/Registrations/Registration/Cautions/Caution', 2)
WITH (pk_caution decimal, 
	CautionType varchar(100),
	CautionText nvarchar(max),
	CautionStatus varchar(100),
	CautionStartDate datetime,
	CautionEndDate datetime,
	CautionEditDate datetime,
	RegistrationId decimal '../../pk_registration')


SELECT *
INTO Condition
FROM OPENXML (@hdoc, 'NRTPM/NRTPM_Practitioners/Person/Registrations/Registration/Conditions/Condition', 2)
WITH (pk_condition decimal, 
	ConditionType varchar(100),
	ConditionText nvarchar(max),
	ConditionStatus varchar(100),
	ConditionStartDate datetime,
	ConditionEndDate datetime,
	ConditionEditDate datetime,
	RegistrationId decimal '../../pk_registration')


SELECT *
INTO Undertaking
FROM OPENXML (@hdoc, 'NRTPM/NRTPM_Practitioners/Person/Registrations/Registration/Undertakings/Undertaking', 2)
WITH (pk_undertaking decimal, 
	UndertakingType varchar(100),
	UndertakingText nvarchar(max),
	UndertakingStatus varchar(100),
	UndertakingStartDate datetime,
	UndertakingEndDate datetime,
	UndertakingEditDate datetime,
	RegistrationId decimal '../../pk_registration')


SELECT *
INTO Specialty
FROM OPENXML (@hdoc, 'NRTPM/NRTPM_Practitioners/Person/Registrations/Registration/Specialtys/Specialty', 2)
WITH (pk_speciality decimal, 
	SpecialtyField varchar(200),
	SpecialtySubType varchar(200),
	RegistrationId decimal '../../pk_registration')


SELECT *
INTO Notation
FROM OPENXML (@hdoc, 'NRTPM/NRTPM_Practitioners/Person/Registrations/Registration/Notations/Notation', 2)
WITH (pk_notation decimal, 
	NotationType varchar(100),
	NotationText nvarchar(max),
	NotationStatus varchar(100),
	NotationOnRegisterFlag int,
	NotationEndDate datetime,
	NotationEditDate datetime,
	RegistrationId decimal '../../pk_registration')


SELECT *
INTO Endorsement
FROM OPENXML (@hdoc, 'NRTPM/NRTPM_Practitioners/Person/Registrations/Registration/Endorsements/Endorsement', 2)
WITH (pk_endorsement decimal, 
	EndorsementType varchar(100),
	EndorsementSubType varchar(100),
	EndorsementText nvarchar(max),
	EndorsementStartDate datetime,
	EndorsementEndDate datetime,
	EndorsementEditDate datetime,
	RegistrationId decimal '../../pk_registration')


SELECT *
INTO Reprimand
FROM OPENXML (@hdoc, 'NRTPM/NRTPM_Practitioners/Person/Registrations/Registration/Reprimands/Reprimand', 2)
WITH (pk_reprimand decimal, 
	ReprimandType varchar(100),
	ReprimandText nvarchar(max),
	ReprimandStatus varchar(100),
	ReprimandStartDate datetime,
	ReprimandEndDate datetime,
	ReprimandEditDate datetime,
	RegistrationId decimal '../../pk_registration')


SELECT *
INTO Address
FROM OPENXML (@hdoc, 'NRTPM/NRTPM_Practitioners/Person/Addresses/Address', 2)
WITH (pk_address decimal, 
	AddressType varchar(20),
	CountryName varchar(200),
	State varchar(100),
	Suburb varchar(200),
	Postcode decimal,
	AddressEditDate datetime,
	PersonId decimal '../../pk_person')


SELECT *
INTO Language
FROM OPENXML (@hdoc, 'NRTPM/NRTPM_Practitioners/Person/Languages/Language', 2)
WITH (pk_language decimal, 
	Language varchar(20),
	PersonId decimal '../../pk_person')

EXEC sp_xml_removedocument @hdoc;

END;

--execute this procedure to read xml data and load into tables
--exec sp_readXmlFile @fileName='C:\sample_pph.xml'
--drop proc sp_readXmlFile
