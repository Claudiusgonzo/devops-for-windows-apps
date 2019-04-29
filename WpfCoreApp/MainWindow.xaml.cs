﻿using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using OSVersionHelper;
using Windows.ApplicationModel;
using Windows.Management.Deployment;
using WpfCoreApp.Telemetry;

namespace WpfCoreApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            versionText.Text = ThisAppInfo.GetDisplayName() + ThisAppInfo.GetThisAssemblyVersion();
            inPackage.Text = WindowsVersionHelper.HasPackageIdentity.ToString();
            deploymentType.Text = ThisAppInfo.GetDotNetInfo();
            packageVersion.Text = ThisAppInfo.GetPackageVersion();
            installedFrom.Text = ThisAppInfo.GetAppInstallerUri();

            DiagnosticsClient.TrackPageView(nameof(MainWindow));
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (ButtonShowRuntimeVersionInfo.Content.ToString().StartsWith("Show"))
            {
                RuntimeVersionInfo.Text = ThisAppInfo.GetDotNetRuntimeInfo();
                DiagnosticsClient.TrackEvent("ClickShowRuntimeInfo");
                ButtonShowRuntimeVersionInfo.Content = "Hide Runtime Info";
            }
            else
            {
                RuntimeVersionInfo.Text = "";
                DiagnosticsClient.TrackEvent("ClickShowRuntimeInfo");
                ButtonShowRuntimeVersionInfo.Content = "Show Runtime Info";
            }
        }

        private async void ButtonCheckForUpdates_Click(object sender, RoutedEventArgs e)
        {
            if (WindowsVersionHelper.HasPackageIdentity)
            {
                var p = Package.Current;
                var updateInfo = await p.CheckUpdateAvailabilityAsync();
                UpdateInfo.Text = updateInfo.Availability.ToString();
            }
            else
            {
                UpdateInfo.Text = "Not packaged, can't check for updates";
            }
            
        }
    }
}
