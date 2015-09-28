using System.IO;
using System.Xml;

namespace D_Accounting
{
    internal class ListCasesXmlReaderParser
    {
        private XmlReader reader = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filePath"></param>
        /// <exception cref="System.IO.DirectoryNotFoundException">If the directory does not exist</exception>
        /// <exception cref="System.IO.FileNotFoundException">If the file does not exist</exception>
        internal ListCasesXmlReaderParser(FileInfo filePath)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;
            settings.IgnoreComments = true;
            settings.IgnoreProcessingInstructions = true;

            try
            {
                reader = XmlReader.Create(filePath.ToString(), settings);
            }
            catch (DirectoryNotFoundException e)
            {
                throw e;
            }
            catch (FileNotFoundException e)
            {
                throw e;
            }
        }

        internal ListCases Read()
        {
            try
            {
                ListCases c = new ListCases(reader);
                reader.Close();

                return c;
            }
            catch (XmlException e)
            {
                throw e;
            }
        }

    }
}
