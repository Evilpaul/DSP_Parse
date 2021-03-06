﻿using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DSP_Parse
{
	class Program
	{
		static private int commandCount = 0;
		static private List<string> parsedLines = new List<string>();

		static void Main(string[] args)
		{
			// Check for correct arguments
			if ((args.Length < 2) || (args.Length > 3))
			{
				Console.WriteLine("Usage: " + System.AppDomain.CurrentDomain.FriendlyName + " <input file> <arrayName> [address byte]");
				Console.WriteLine(args.Length);
				return;
			}

			// Check that array name is valid
			if ((args[1].Contains(" ")) || (Regex.IsMatch(args[1], @"^\d")))
			{
				Console.WriteLine("Invalid array name");
				return;
			}

			try
			{
				if (File.Exists(args[0]))
				{
					Console.WriteLine("/* Data generated " + DateTime.Now.ToString("G") + " from \"" + args[0] + "\" */");

					//Create the RichTextBox. (Requires a reference to System.Windows.Forms.)
					System.Windows.Forms.RichTextBox rtBox = new System.Windows.Forms.RichTextBox();

					// Get the contents of the RTF file. When the contents of the file are   
					// stored in the string (rtfText), the contents are encoded as UTF-16.  
					string rtfText = System.IO.File.ReadAllText(args[0]);

					// Use the RichTextBox to convert the RTF code to plain text.
					rtBox.Rtf = rtfText;

					foreach (string line in rtBox.Lines)
					{
						string the_line = line.Trim();
						if (the_line.EndsWith("*/"))
						{
							if (commandCount > 0)
							{
								endBlock();
							}

							// add the comment above the array
							parsedLines.Add(the_line.Substring(the_line.IndexOf("/*")));

							parsedLines.Add("static const uint8 " + args[1] + commandCount + "[] = {");
							commandCount++;

							// add address to start of data
							if (args.Length == 3)
							{
								string temp_str = args[2] + ", ";
								the_line = string.Concat(temp_str, the_line);
							}

							// remove comment from line
							the_line = the_line.Remove(the_line.IndexOf("/*")).Trim();
						}

						if(!String.IsNullOrWhiteSpace(the_line))
						{
							parsedLines.Add(the_line);
						}
					}

					endBlock();
				}
				else
				{
					Console.WriteLine("File does not exist");
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("The process failed: {0}", e.ToString());
			}
		}

		static void endBlock()
		{
			// Remove the end comma from the last data line
			for (int i = parsedLines.Count - 1; i > 0; i--)
			{
				if (parsedLines[i].EndsWith(","))
				{
					parsedLines[i] = parsedLines[i].TrimEnd(',');
					break;
				}
			}
			parsedLines.Add("};");
			parsedLines.Add("");

			foreach (string out_line in parsedLines)
			{
				Console.WriteLine(out_line);
			}

			parsedLines = new List<string>();
		}
	}
}
