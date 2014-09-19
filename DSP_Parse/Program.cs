using System;
using System.Text;
using System.Windows.Forms;

namespace DSP_Parse
{
	class Program
	{
		static private int commandCount = 0;

		static void Main(string[] args)
		{
			// If your RTF file isn't in the same folder as the .exe file for the project,  
			// specify the path to the file in the following assignment statement.  
			string path = @"test.rtf";

			//Create the RichTextBox. (Requires a reference to System.Windows.Forms.)
			System.Windows.Forms.RichTextBox rtBox = new System.Windows.Forms.RichTextBox();

			// Get the contents of the RTF file. When the contents of the file are   
			// stored in the string (rtfText), the contents are encoded as UTF-16.  
			string rtfText = System.IO.File.ReadAllText(path);

			// Use the RichTextBox to convert the RTF code to plain text.
			rtBox.Rtf = rtfText;

			foreach (string line in rtBox.Lines)
			{
				string parsed_line = line.Trim();
				if (parsed_line.EndsWith("*/"))
				{
					if (commandCount > 0)
					{
						// remove trailing comma from last command

						Console.WriteLine("};");
						Console.WriteLine("");
					}
					Console.WriteLine("DSPMainData" + commandCount + " = {");
					commandCount++;

					// remove comment from line
				}

				if(!String.IsNullOrWhiteSpace(parsed_line))
				{
					Console.WriteLine(parsed_line);
				}
			}
		}
	}
}
