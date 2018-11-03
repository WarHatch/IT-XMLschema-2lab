using System;
using System.Xml;
using System.Xml.Schema;

public class XsdValidate
{

#pragma warning disable CS0618 // Type or member is obsolete
    static XmlSchemaCollection sc = new XmlSchemaCollection();
#pragma warning restore CS0618 // Type or member is obsolete
    static string xsdFile = null;
    static string xmlFile = null;
    static string nsUri = null;

    static readonly string usage = @"Usage: xsdvalidate.exe [-xml <xml-file>] 
   [-xsd <schema-file>] [-ns <namespace-uri>]

Sample:  xsdvalidate.exe -xml t.xml
Validate the XML file by loading it into XmlValidatingReader with
   ValidationType set to auto.  

Sample:  xsdvalidate.exe -xml t.xml -xsd t.xsd -ns ns1
This will validate the t.xml with the schema t.xsd with target namespace 'ns1'

Sample:  xsdvalidate.exe xsd t.xsd -ns ns1
This will validate the schema t.xsd with target namespace 'ns1'";

    public static void ValidationCallback(object sender, ValidationEventArgs args)
    {

        if (args.Severity == XmlSeverityType.Warning)
            Console.Write("WARNING: ");
        else if (args.Severity == XmlSeverityType.Error)
            Console.Write("ERROR: ");

        Console.WriteLine(args.Message); // Print the error to the screen.
    }

    public static void Main(string[] args)
    {

        if ((args.Length == 0) || (args.Length % 2 != 0))
        {
            Console.WriteLine(usage);
            return;
        }

        for (int i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {

                case "-xsd": xsdFile = args[++i]; break;
                case "-xml": xmlFile = args[++i]; break;
                case "-ns": nsUri = args[++i]; break;

                default: Console.WriteLine("ERROR: Unexpected argument " + args[i]); return;

            }//switch
        }//for

        if (xsdFile != null)
        {
            sc.ValidationEventHandler += new ValidationEventHandler(ValidationCallback);
            sc.Add(nsUri, xsdFile);
            Console.WriteLine("Schema Validation Completed");
        }

        if (xmlFile != null)
        {
#pragma warning disable CS0618 // Type or member is obsolete
            XmlValidatingReader vr = new XmlValidatingReader(new XmlTextReader(xmlFile));
#pragma warning restore CS0618 // Type or member is obsolete
            vr.Schemas.Add(sc);
            vr.ValidationType = ValidationType.Schema;
            vr.ValidationEventHandler += new ValidationEventHandler(ValidationCallback);

            while (vr.Read()) ;
            Console.WriteLine("Instance Validation Completed");
        }
    }//Main
}//XsdValidate