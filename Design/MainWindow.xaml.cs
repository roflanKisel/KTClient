using System;
using System.Windows;
using MahApps.Metro.Controls;
using KTClient.Logic;
using MahApps.Metro.Controls.Dialogs;
using System.Net.Sockets;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using MahApps.Metro;
using System.Threading.Tasks;

namespace KTClient
{
    public partial class MainWindow : MetroWindow
    {
        private Dictionary<string, string> body;

        // init variables in constructor
        public MainWindow()
        {
            InitializeComponent();
            this.body = new Dictionary<string, string>();
            this.requestMethodBox.SelectedIndex = 0;
            this.requestOptionsTabControl.SelectedIndex = 1;
        }
        
        private void sendButton_Click(object sender, RoutedEventArgs e)
        {
            this.sendInfo();
        }

        // sending info to server and handling response
        private void sendInfo()
        {
            // hanle URI
            string stringUri = this.pathTextBox.Text;
            UriResolver uriResolver = new UriResolver(stringUri);
            Uri uri = uriResolver.getUri();
            // checking URI
            if (uri != null && uriResolver.getIPAddresses() != null)
            {
                string requestString = formRequestString(uri);
                this.showHeaders(uri);
                try
                {
                    // create sending data thread
                    Task.Factory.StartNew(() =>
                    {
                        // send data to server
                        string response = ConnectionService.sendData(uri, uriResolver.getIPAddresses(), requestString);
                        Dispatcher.Invoke(() =>
                        {
                            // pick body from response
                            this.responseBodyTextBlock.Text = MessageParser.getBodyFromMessage(response);
                            // pick headers from response
                            this.responseHeadersTextBlock.Text = MessageParser.getHeadersFromMessage(response);
                            // write response body to file
                            File.WriteAllText("..\\..\\Resources\\Web\\temp-page.html", MessageParser.getBodyFromMessage(response));
                        });
                    });
                   
                }
                catch (SocketException exception)
                {

                    this.ShowMessageAsync("Error", exception.ToString());
                }
            }
            else
            {
                this.ShowMessageAsync("Error", "Name not resolved");
            }
        }

        private void showHeaders(Uri uri)
        {
            string headers = formHeaders(uri, formBody().Length);
            this.headersLabel.Content = headers.Substring(headers.IndexOf("\r\n") + 2);
        }

        // form request
        private string formRequestString(Uri uri)
        {
            string body = formBody();
            string headers = formHeaders(uri, body.Length) + "\r\n";
            return headers + body;
        }

        // create headers for request
        private string formHeaders(Uri uri, int contentLength)
        {
            string headers = string.Empty;
            string requestMethod = this.requestMethodBox.SelectionBoxItem.ToString();
            headers += requestMethod + " " + uri.PathAndQuery + " HTTP/1.1\r\n";
            headers += "Host: " + uri.Host + "\r\n";
            headers += "User-Agent: KTClient\r\n";
            headers += "Content-Length: " + contentLength + "\r\n";
            headers += "Connection: keep-alive\r\n";
            return headers;
        }

        // create body for request
        private string formBody()
        {
            if (this.requestMethodBox.SelectionBoxItem.ToString() == "GET")
            {
                return "";
            }
            string body = string.Empty;
            foreach (var item in this.body)
            {
                body += item.Key.ToString() + "=" + item.Value.ToString() + "&";
            }
            if (body.Length > 0) {
                body = body.Substring(0, body.Length - 1);
            }
            return body;
        }

        // open request code
        private void viewCodeButton_Click(object sender, RoutedEventArgs e)
        {
            string stringUri = this.pathTextBox.Text;
            UriResolver uriResolver = new UriResolver(stringUri);
            Uri uri = uriResolver.getUri();

            if (uri != null && uriResolver.getIPAddresses() != null)
            {
                this.codeFlyoutLabel.Content = this.formRequestString(uri);
                this.codeFlyout.IsOpen = true;
            } else
            {
                this.codeFlyoutLabel.Content = "Bad request";
                this.codeFlyout.IsOpen = true;
            }
        }

        // open created file in browser
        private void webViewButton_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("file:///D:/Study/4_sem/KSIS/KTProject/KTClient/Resources/Web/temp-page.html");
        }

        // add button handler
        private void addBodyVariableButton_Click(object sender, RoutedEventArgs e)
        {
            // remove spaces
            String variable = this.variableCreatorBox.Text.Trim();
            String value = this.valueCreatorBox.Text.Trim();
            try
            {
                if (variable != "" && value != "")
                {
                    this.body.Add(variable, value);
                    this.bodyVariableCombobox.Items.Add(variable);
                    this.bodyVariableCombobox.SelectedIndex = this.bodyVariableCombobox.Items.Count - 1;
                }
                else
                {
                    this.ShowMessageAsync("Error", "Some elements are empty");
                }
            }
            catch (Exception)
            {
                this.ShowMessageAsync("Error", "Body element has already been added");
            }
            finally
            {
                this.variableCreatorBox.Text = "";
                this.valueCreatorBox.Text = "";
            }
        }

        // edit button handler
        private void editBodyVariableButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.bodyVariableCombobox.SelectedIndex != -1)
                {
                    String value = this.bodyVariableCombobox.SelectionBoxItem.ToString();
                    if (this.bodyValueForCombobox.Text.Trim() != "")
                    {
                        this.body[value] = this.bodyValueForCombobox.Text;
                    }
                    else
                    {
                        this.ShowMessageAsync("Error", "Invalid value");
                    }
                }
                else
                {
                    this.ShowMessageAsync("Error", "There is no selected item");
                    this.bodyValueForCombobox.Text = "";
                }
            }
            catch (Exception)
            {
                Console.WriteLine("There is no selected item");
            }
        }

        // delete variable
        private void deleteBodyVariableButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.bodyVariableCombobox.SelectedIndex != -1)
                {
                    // find by selected index
                    object value = this.bodyVariableCombobox.Items.GetItemAt(this.bodyVariableCombobox.SelectedIndex);
                    this.bodyVariableCombobox.Items.Remove(value);
                    this.bodyVariableCombobox.Items.Refresh();
                    this.bodyVariableCombobox.SelectedIndex = 0;
                    body.Remove(value.ToString());
                }
                else
                {
                    this.ShowMessageAsync("Error", "There is no selected item");
                    this.bodyValueForCombobox.Text = "";
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Selected item index = " + this.bodyVariableCombobox.SelectedIndex);
            }
        }

        // variable change listener
        private void bodyVariableCombobox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.bodyVariableCombobox.SelectedIndex != -1)
                {
                    String value = this.bodyVariableCombobox.Items.GetItemAt(this.bodyVariableCombobox.SelectedIndex).ToString().Trim();
                    this.bodyValueForCombobox.Text = this.body[value];
                } else
                {
                    this.bodyValueForCombobox.Text = "";
                    this.bodyVariableCombobox.SelectedIndex = 0;
                }
            } catch (Exception)
            {
                Console.WriteLine("Body combobox selection exception");
            }
        }

        private void settingsButton_Click(object sender, RoutedEventArgs e)
        {
            this.settingsFlyout.IsOpen = !this.settingsFlyout.IsOpen;
        }

        // toggle theme
        private void ToggleSwitch_IsCheckedChanged(object sender, EventArgs e)
        {
            if (this.themeSwitcher.IsChecked.Value)
            {
                ThemeManager.ChangeAppStyle(Application.Current,
                                    ThemeManager.GetAccent("Green"),
                                    ThemeManager.GetAppTheme("BaseLight"));
            } else
            {
                ThemeManager.ChangeAppStyle(Application.Current,
                                    ThemeManager.GetAccent("Cyan"),
                                    ThemeManager.GetAppTheme("BaseDark"));
            }
        }

        // enter listening on url line
        private void pathTextBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                this.sendInfo();
            }
        }
    }
}
