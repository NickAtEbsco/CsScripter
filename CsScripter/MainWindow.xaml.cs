using System;
using System.Configuration;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using CsScripterLib;
using Microsoft.Win32;

namespace CsScripter
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private string m_filename = null;
		private string m_baseTitle = null;
		private bool m_runOnReturn = false;

		private IInterpreter m_interpreter;
		private IParserFunction m_parserFunction;

		public MainWindow(IInterpreter theInterpreter, IParserFunction theParserFunction)
		{
			InitializeComponent();

			m_interpreter = theInterpreter;
			m_parserFunction = theParserFunction;

			textSource.Focus();
			m_baseTitle = this.Title + " - ";
			OpenLastIfPossible();

			//textSource.Text = "; This is a comment\nprint(\"Hello World\")";
			//textSource.Text = "; This is a comment\nset(text,\"Hello World\")\nprint(text)";
			//textSource.Text = "; This is a comment\nset(text,\"Hello World\"\nprint(text)";
			//textSource.Text = "; This is a comment\nprint(\"text)";
			//textSource.Text = "; This is a comment\nset(sum,1+1)\nprint(sum)";

			textSource.Text = "; This is a comment\n:myVar=3+10/2-3\nprint(:myVar)";
			//textSource.Text = "; This is a comment\nprint(3+10/2-3)";

			// Temporary for now.  Long term this would be IoC.
			m_interpreter.Initialize();
		}

		private void Open(object sender, RoutedEventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.DefaultExt = "cscs";
			openFileDialog.Filter = "cscs files (*.cscs)|*.cscs|All files (*.*)|*.*";
			openFileDialog.FilterIndex = 0;
			openFileDialog.RestoreDirectory = true;

			if (openFileDialog.ShowDialog() == true)
			{
				try
				{
					m_filename = openFileDialog.FileName;
					string[] readText = File.ReadAllLines(m_filename);
					textSource.Text = string.Join("\n", readText);

					this.Title = m_baseTitle + openFileDialog.SafeFileName;
					SaveFilename(m_filename);
				}
				catch (Exception ex)
				{
					MessageBox.Show("Couldn't read file from disk. Original error: " + ex.Message);
				}
			}
		}
		private void Save(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrWhiteSpace(m_filename))
			{
				SaveAs(sender, e);
				return;
			}
			SaveFile();
		}

		private void SaveAs(object sender, RoutedEventArgs e)
		{
			SaveFileDialog saveFileDialog = new SaveFileDialog();
			saveFileDialog.DefaultExt = "cscs";
			saveFileDialog.Filter = "cscs files (*.cscs)|*.cscs|All files (*.*)|*.*";
			saveFileDialog.FilterIndex = 0;
			saveFileDialog.RestoreDirectory = true;

			if (saveFileDialog.ShowDialog() == true)
			{
				m_filename = saveFileDialog.FileName;
				SaveFile();

				this.Title = m_baseTitle + saveFileDialog.SafeFileName;
				SaveFilename(m_filename);
			}
		}

		private void SaveFile()
		{
			try
			{
				File.WriteAllText(m_filename, textSource.Text);

				Paragraph p = new Paragraph(new Run("File " + m_filename + " saved."));
				p.Foreground = Brushes.Black;
				textResult.Document.Blocks.Add(p);
			}
			catch (Exception ex)
			{
				MessageBox.Show("Couldn't write file to disk. Original error: " + ex.Message);
			}
		}

		private void SaveFilename(string filename)
		{
			Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
			config.AppSettings.Settings["lastFilename"].Value = filename;
			config.Save(ConfigurationSaveMode.Minimal);
			//ConfigurationManager.RefreshSection("appSettings");
		}

		private string OpenLastFilename()
		{
			var fileConfig = ConfigurationManager.AppSettings["lastFilename"];
			if (fileConfig == null)
			{
				return null;
			}
			return fileConfig;
		}

		private void OpenLastIfPossible()
		{
			m_filename = OpenLastFilename();
			if (string.IsNullOrWhiteSpace(m_filename))
			{
				return;
			}

			try
			{
				string[] readText = File.ReadAllLines(m_filename);
				textSource.Text = string.Join("\n", readText);

				this.Title = m_baseTitle + Path.GetFileName(m_filename);
				SaveFilename(m_filename);
			}
			catch (Exception)
			{
			}
		}

		private void Clear(object sender, RoutedEventArgs e)
		{
			string text = new TextRange(textResult.Document.ContentStart, textResult.Document.ContentEnd).Text;
			if (string.IsNullOrWhiteSpace(text))
			{
				textSource.Text = string.Empty;
			}
			else
			{
				textResult.Document.Blocks.Clear();
			}
		}

		private void Run(object sender, RoutedEventArgs e)
		{
			try
			{
				//Parser.Verbose = true;

				//string script = "if(2<=1){print(2+2-(3-1));if(4>2){set(www,500);set(ww,50);print(3*www-ww*2-(-3+sin(pi/2)));print(\"IF DONE\");}}else{print(\"ELSE\");set(www,env(path));print(www);print(\"I am in ELSE\");}print(\"We are DONE.\")";
				//string script = "set(b,env(path));set(env1,\"www.ilanguage.ch\");set(env2,\"Skype\");if(indexOf(b,env2)<0){append(b,env1);}print(b);";
				//string script = "set(i,1);set(b,\"Argument\");unless(i>5){append(b,2*i);print(i);print(b);set(i,i+1);}print(\"Done!\");";

				string script = textSource.SelectedText;
				if (m_runOnReturn)
				{
					script = textSource.GetLineText(textSource.GetLineIndexFromCharacterIndex(textSource.SelectionStart - 1));
				}
				else
				{
					textResult.Document.Blocks.Clear();
				}

				if (string.IsNullOrWhiteSpace(script))
				{
					script = textSource.Text;
				}
				string errorMsg = null;
				try
				{
					m_interpreter.Process(script);
				}
				catch (ArgumentException exc)
				{
					errorMsg = exc.Message;
				}

				textResult.Foreground = Brushes.Green;

				string output = m_interpreter.Output;
				if (!string.IsNullOrWhiteSpace(output))
				{
					textResult.Document.Blocks.Add(new Paragraph(new Run(output)));
				}
				if (!string.IsNullOrWhiteSpace(errorMsg))
				{
					Paragraph p = new Paragraph(new Run(errorMsg));
					p.Foreground = Brushes.Red;
					textResult.Document.Blocks.Add(p);
				}
			}
			catch( Exception ex )
			{
				textResult.Document.Blocks.Add(new Paragraph(new Run(ex.ToString())));
			}
		}

		private void RunCheckbox(object sender, RoutedEventArgs e)
		{
			m_runOnReturn = runOnReturn.IsChecked == true;
			//textSource.
		}

		private void TextBox_KeyEnterUpdate(object sender, KeyEventArgs e)
		{
			if (m_runOnReturn && e.Key == Key.Enter)
			{
				TextBox tBox = (TextBox)sender;
				Run(sender, e);
			}
		}
	}
}
